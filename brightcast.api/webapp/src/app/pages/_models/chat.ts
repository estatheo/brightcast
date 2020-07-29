export class ChatMessage {
    id?: number;
    senderId?: number;
    senderName?: string;
    avatarUrl?: string;
    type?: string;
    reply?: boolean;
    files?: any;
    text?: string;
    createdAt?: Date;
    campaignId?: number;
    contactListId?: number;
    status?: number;
}
