export interface IMessageTopic {
    id: number;
    name: string;
}

export class MessageTopic {
    public id: number;
    public name: string;
    constructor(id: number, name: string) {
        this.id = id;
        this.name = name;
    }
}