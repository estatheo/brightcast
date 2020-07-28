import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NbMenuService, NbMenuItem } from '@nebular/theme';
import { ChatService } from './chat.service';
import { CampaignService} from '../../../@core/apis/campaign.service';
import { CampaignData } from '../../_models/campaignData';
import { ContactListElement } from '../../_models/contactListElement';
import { ContactService } from '../../../@core/apis/contact.service';
import { Contact } from '../../_models/contact';
import { AccountService } from '../../../pages/_services';
import { UserProfile } from '../../../pages/_models/userProfile';
import { map, takeUntil } from 'rxjs/operators';

@Component({
  selector: 'ngx-chat',
  templateUrl: 'chat.component.html',
  styleUrls: ['chat.component.scss'],
  providers: [ ChatService ],
})
export class ChatComponent implements OnInit {
  chat_menu: Array<NbMenuItem> = [];
  messages: any[];
  campaign_data: CampaignData;
  chat_title: string;
  chat_themes = ['success', 'danger', 'primary', 'info', 'warning'];
  chat_theme: string;
  campaign_id: number;
  customer_list: Contact[];
  user: UserProfile;

  constructor(
    protected chatService: ChatService,
    private menuService: NbMenuService,
    private campaignsService: CampaignService,
    private contactService: ContactService,
    private userService: AccountService,
    private route: ActivatedRoute, private router: Router) {
    this.messages = this.chatService.loadMessages();
  }

  ngOnInit(): void {
    this.chat_title = 'Chat';
    this.chat_theme = this.chat_themes[Math.floor(Math.random() * 5)];

    this.route.params.subscribe(p => {
      this.campaign_id = parseInt(p['id'], 10);
    });
    
    this.userService.getUserProfile()
      .subscribe((user: UserProfile) => this.user = user);

    this.campaignsService.refreshData();
    this.campaignsService.data.subscribe((data: CampaignData) => {
      this.campaign_data = data;
      this.campaign_data.campaigns.forEach((campaign_item) => {
        if (campaign_item.id === this.campaign_id) {
          this.chat_title = 'Chat for \"' + campaign_item.name + '\"';
          this.contactService.SetContactListId(campaign_item.contactListIds[0]);
          this.contactService.data.subscribe((contactlist: Contact[]) => {
            this.contactService.refreshData();
            this.customer_list = contactlist;
          });
        }
        this.chat_menu.push({
          title: campaign_item.name,
          url: '/pages/main/campaign/chat/' + campaign_item.id,
        });
      });
    });
  }

  sendMessage(event: any) {
    const files = !event.files ? [] : event.files.map((file) => {
      return {
        url: file.src,
        type: file.type,
        icon: 'nb-compose',
      };
    });

    this.messages.push({
      text: event.message,
      date: new Date(),
      reply: true,
      type: files.length ? 'file' : 'text',
      files: files,
      user: {
        name: this.user.firstName + ' ' + this.user.lastName,
        avatar: this.user.pictureUrl,
      },
    });
    const botReply = this.chatService.reply(event.message);
    if (botReply) {
      setTimeout(() => { this.messages.push(botReply); }, 500);
    }
  }
}
