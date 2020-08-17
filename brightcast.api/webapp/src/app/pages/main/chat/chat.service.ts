import { EventEmitter, Injectable } from '@angular/core';

import { messages } from './messages';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { map } from 'rxjs/operators';
import { HubConnection, HubConnectionBuilder, HttpTransportType } from '@aspnet/signalr';
import { ChatMessage } from '../../_models/chat';
import { Invitation } from '../../_models/invitation';

@Injectable()
export class ChatService {

  apiURL: string = environment.apiUrl;
  messageReceived = new EventEmitter<ChatMessage>();

  private _hubConnection: HubConnection;

  constructor(private httpClient: HttpClient) {
  }

  sendMessage(message: ChatMessage) {
    this._hubConnection.invoke('NewMessage', message);
  }

  startConnection = () => {
    Object.defineProperty(WebSocket, 'OPEN', { value: 1 });
    this._hubConnection = new HubConnectionBuilder()
        .withUrl(this.apiURL + '/ChatHub',
          { transport: HttpTransportType.WebSockets | HttpTransportType.LongPolling })
        .build();

    this._hubConnection.start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }

  registerOnServerEvents(): void {
    this._hubConnection.on('MessageReceived', (data: any) => {
      this.messageReceived.emit(data);
    });
  }

  loadMessagesByCampaignAndContactId(campaignId: number, contactId: number) {
    return this.httpClient.get(`${this.apiURL}/chat/ofCampaignAndContact/${campaignId}/${contactId}/`).pipe(map(response => response));
  }

  newChatMessage(newMessage: ChatMessage) {
    return this.httpClient.post(`${this.apiURL}/chat/new`, newMessage).pipe(map(response => response));
  }

  sendInviteMessage(invitation: Invitation) {
    return this.httpClient.post(`${this.apiURL}/chat/sendInvitation`, invitation).pipe(map(response => response));
  }

  loadMessages() {
    return messages;
  }
}
