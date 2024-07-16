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

export class Message2 {
    public topicId: number;
    public contactId: number;
    public messageText: string;
    public postDate: any;

    constructor(topicId: number, contactId: number, messageText: string, postDate: any) {
        this.topicId = topicId;
        this.contactId = contactId;
        this.messageText = messageText;
        this.postDate = postDate;
    }
}