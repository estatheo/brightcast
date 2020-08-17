export class ChatMessage {
    id?: number;
    senderId?: number;
    senderName?: string;
    receiverId?: number;
    receiverName?: string;
    receiverPhone?: string;
    avatarUrl?: string;
    type?: string;
    reply?: boolean;
    files?: string;
    text?: string;
    createdAt?: Date;
    campaignId?: number;
    contactId?: number;
}
