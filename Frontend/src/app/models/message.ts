import { IContact, TContact } from "./contact";
import { IMessageTopic, TMessageTopic } from "./messagetopic";

export type TMessageDTO = {
    contactName: string,
    contactMail : string,
    contactPhone: string,
    topicId: number,
    messageText: string,
}

export type TMessage = {
    id: number;
    topicId: number;
    contactId: number;
    messageText: string;
    postDate: any;
}

export type TMessageFull = {
    contact: TContact,
    message: IMessage,
    messageTopic: IMessageTopic,
}

export interface IMessageFull {
        contact: TContact;
        message: TMessage;
        messageTopic: TMessageTopic;
}

export interface IMessage {
    id: number;
    topicId: number;
    contactId: number;
    messageText: string;
    postDate: any;
}

export class Message {
    public id: number;
    public topicId: number;
    public contactId: number;
    public messageText: string;
    public postDate: any;

    constructor(id: number, topicId: number, contactId: number, messageText: string, postDate: any) {
        this.id = id;
        this.topicId = topicId;
        this.contactId = contactId;
        this.messageText = messageText;
        this.postDate = postDate;
    }
}