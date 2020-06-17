using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using brightcast.Entities;
using brightcast.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace brightcast.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
        Task SendConfirmationEmail(int userId, string email);
        void ActivateUser(Guid code);
        Guid? RequestResetPassword(string email);
        Task SendResetPasswordEmail(Guid code, string email);
        void ResetPassword(Guid code, string password);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;

        public UserService(DataContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Username == username && x.Deleted == 0);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.Where(x => x.Deleted == 0);
        }

        public User GetById(int id)
        {
            var user = _context.Users.Find(id);

            return user != null && user.Deleted == 0 ? user : null;
        }

        public User Create(User user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.Username == user.Username))
                throw new AppException("Username \"" + user.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            user.CreatedAt = DateTime.UtcNow;
            user.CreatedBy = "API";

            user.Deleted = 1;

            var code = Guid.NewGuid();
            
            var createdUser = _context.Users.Add(user);
            _context.SaveChanges();

            _context.UserActivations.Add(new UserActivation()
            {
                ActivationCode = code,
                Activated = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "API",
                Deleted = 0,
                UserId = user.Id
            });
            _context.SaveChanges();

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.Id);

            if (user == null || user.Deleted == 1)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.Users.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");

                user.Username = userParam.Username;
            }

            // update user properties if provided

            user.UpdatedBy = userParam.UpdatedBy;

            user.UpdatedAt = userParam.UpdatedAt;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.Deleted = 1;
                
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        public void ActivateUser(Guid code)
        {
            var userActivation = _context.UserActivations.FirstOrDefault(x => x.ActivationCode == code && x.Deleted == 0 && x.CreatedAt.AddDays(1) >= DateTime.UtcNow);
            if (userActivation != null)
            {
                userActivation.Activated = true;
                _context.UserActivations.Update(userActivation);

                var user = _context.Users.Find(userActivation.UserId);
                user.Deleted = 0;
                _context.Users.Update(user);

                _context.SaveChanges();
            }
        }

        public Guid? RequestResetPassword(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == email && x.Deleted == 0);
            if (user != null)
            {
                var code = Guid.NewGuid();
                _context.ResetPasswords.Add(new ResetPassword
                {
                    ResetCode = code,
                    UserId = user.Id,
                    Activated = false,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "API",
                    Deleted = 0
                });

                _context.SaveChanges();

                return code;
            }

            return null;
        }


        public void ResetPassword(Guid code, string password)
        {
            var resetPassword = _context.ResetPasswords.FirstOrDefault(x =>
                x.ResetCode == code && x.Deleted == 0 && x.CreatedAt.AddDays(1) >= DateTime.UtcNow);

            if (resetPassword != null)
            {
                resetPassword.Activated = true;
                _context.ResetPasswords.Update(resetPassword);


                if (!string.IsNullOrWhiteSpace(password))
                {
                    byte[] passwordHash, passwordSalt;
                    CreatePasswordHash(password, out passwordHash, out passwordSalt);

                    var user = _context.Users.Find(resetPassword.UserId);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    
                    _context.Users.Update(user);
                    _context.SaveChanges();
                }
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public async Task SendConfirmationEmail(int userId, string email)
        {
            var activationModel = _context.UserActivations.FirstOrDefault(x => x.UserId == userId);
            if (activationModel != null)
            {
                var code = activationModel.ActivationCode;

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appSettings.SendgridApiKey);
                var p = new 
                {
                    personalizations = new Object[]
                    {
                        new
                        {
                            to = new Object[]
                            {
                                new
                                {
                                    email = email,
                                    name = email
                                }
                            },
                            dynamic_template_data = new 
                            {
                                url = $"{_appSettings.UiBaseUrl}/auth/verify/{code}"
                            },
                            subject = "Verify your Brightcast account now!"
                        }
                    },
                    from = new
                    {
                        email = "hello@brightcast.io",
                        name = "Brightcast.io"
                    },
                    reply_to = new
                    {
                        email = "hello@brightcast.io",
                        name = "Brightcast.io"
                    },
                    template_id = "d-7836d34cf64446278de789db1ccdcff8"

                };
                var httpRsponse = await httpClient.PostAsync("https://api.sendgrid.com/v3/mail/send",
                    new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json"));
            }
        }

        public async Task SendResetPasswordEmail(Guid code, string email)
        {
           
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appSettings.SendgridApiKey);
            var p = new
            {
                personalizations = new Object[]
                {
                    new
                    {
                        to = new Object[]
                        {
                            new
                            {
                                email = email,
                                name = email
                            }
                        },
                        dynamic_template_data = new
                        {
                            url = $"{_appSettings.UiBaseUrl}/auth/new-password/{code}"
                        },
                        subject = "Reset the password of your Brightcast account!"
                    }
                },
                from = new
                {
                    email = "hello@brightcast.io",
                    name = "Brightcast.io"
                },
                reply_to = new
                {
                    email = "hello@brightcast.io",
                    name = "Brightcast.io"
                },
                template_id = "d-91fc8978fccc4bb0b4938b1acad7eed9"

            };
            var httpRsponse = await httpClient.PostAsync("https://api.sendgrid.com/v3/mail/send",
                new StringContent(JsonConvert.SerializeObject(p), Encoding.UTF8, "application/json"));
        }
    }
}