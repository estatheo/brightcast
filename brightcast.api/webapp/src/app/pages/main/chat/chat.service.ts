import { Injectable } from '@angular/core';

import { messages } from './messages';
import { botReplies, gifsLinks, imageLinks } from './bot-replies';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { of ,  Observable } from 'rxjs';
import { shareReplay, map, refCount, publishReplay } from 'rxjs/operators';
@Injectable()
export class ChatService {

  apiURL: string = environment.apiUrl;
  private cache$: Observable<Object>;

  constructor(private httpClient: HttpClient) {
  }

  get data() {
    if ( !this.cache$ ) {
      this.cache$ = this.requestData().pipe(
        publishReplay(1),
        refCount(),
      );
    }
    return this.cache$;
  }

  refreshData() {
    this.cache$ = null;
  }

  private requestData() {
    return this.httpClient.get(`${this.apiURL}/contact/ofList/`).pipe(map(response => response));
  }

  loadMessages() {
    return messages;
  }

  loadBotReplies() {
    return botReplies;
  }

  reply(message: string) {
    const botReply: any =  this.loadBotReplies()
      .find((reply: any) => message.search(reply.regExp) !== -1);

    if (botReply.reply.type === 'quote') {
      botReply.reply.quote = message;
    }

    if (botReply.type === 'gif') {
      botReply.reply.files[0].url = gifsLinks[Math.floor(Math.random() * gifsLinks.length)];
    }

    if (botReply.type === 'pic') {
      botReply.reply.files[0].url = imageLinks[Math.floor(Math.random() * imageLinks.length)];
    }

    if (botReply.type === 'group') {
      botReply.reply.files[1].url = gifsLinks[Math.floor(Math.random() * gifsLinks.length)];
      botReply.reply.files[2].url = imageLinks[Math.floor(Math.random() * imageLinks.length)];
    }

    botReply.reply.text = botReply.answerArray[Math.floor(Math.random() * botReply.answerArray.length)];
    return { ...botReply.reply };
  }
}
