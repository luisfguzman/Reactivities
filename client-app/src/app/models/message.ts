export interface IMessage {
  id: string;
  senderUserName: string;
  senderDisplayName: string;
  senderPhotoUrl: string;
  recipientUserName: string;
  recipientDisplayName: string;
  recipientPhotoUrl: string;
  content: string;
  isRead: boolean;
  dateRead: Date;
  messageSent: Date;
}