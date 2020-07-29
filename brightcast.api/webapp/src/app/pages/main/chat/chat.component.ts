import { Component, OnInit, NgZone } from '@angular/core';
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
import { ChatMessage } from '../../_models/chat';

@Component({
  selector: 'ngx-chat',
  templateUrl: 'chat.component.html',
  styleUrls: ['chat.component.scss'],
  providers: [ ChatService ],
})
export class ChatComponent implements OnInit {
  chat_menu: Array<NbMenuItem> = [];
  messages: ChatMessage[] = [];
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
    private _ngZone: NgZone,
    private route: ActivatedRoute, private router: Router) {
    // this.messages = this.chatService.loadMessages();
    this.subscribeToEvents();
  }

  ngOnInit(): void {
    this.chatService.startConnection();
    this.chatService.registerOnServerEvents();
    this.chat_title = 'Chat';
    this.chat_theme = this.chat_themes[Math.floor(Math.random() * 5)];

    this.route.params.subscribe(p => {
      this.campaign_id = parseInt(p['id'], 10);
    });

    this.userService.getUserProfile()
      .subscribe((user: UserProfile) => {this.user = user; console.log(this.user); });

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

    const tempMsg = new ChatMessage();
    tempMsg.text = event.message;
    tempMsg.createdAt = new Date();
    tempMsg.reply = true;
    tempMsg.type = files.length ? 'file' : 'text';
    tempMsg.files = files;
    tempMsg.senderId = this.user.id;
    tempMsg.senderName = this.user.firstName + ' ' + this.user.lastName;
    tempMsg.avatarUrl = this.user.pictureUrl;
    tempMsg.campaignId = this.campaign_id;
    tempMsg.contactListId = this.contactService.contactListId;
    this.messages.push(tempMsg);
    this.chatService.sendMessage(tempMsg);
    console.log('messages', this.messages);
  }

  private subscribeToEvents(): void {

    this.chatService.messageReceived.subscribe((message: ChatMessage) => {
      this._ngZone.run(() => {
        message.reply = true;
        if (message.senderId !== this.user.id) {
          message.reply = false;
          this.messages.push(message);
        }
      });
    });
  }
}
