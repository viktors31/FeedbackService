export class NewMessage {
    constructor(
        public contactName: string,
        public contactMail: string,
        public contactPhone: string,
        public topicId: number,
        public messageText: string,
    ) {}
}