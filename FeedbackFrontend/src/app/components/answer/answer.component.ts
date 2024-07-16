import { Component, Input } from '@angular/core';
import { MessageTopic } from '../../models/messagetopic';
import { Contact, IContact } from '../../models/contact';
import { IMessage, Message } from '../../models/message';
import { CommonModule } from '@angular/common';

interface ImessageDbFrame {
  topic?: MessageTopic,
  message?: Message,
  contact?: Contact,
}
export interface IMessageTopic {
  id?: number;
  name?: string;
}

@Component({
  selector: 'app-answer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './answer.component.svg',
  styleUrl: './answer.component.css'
})


export class AnswerComponent {
  @Input() topic!: IMessageTopic;
  @Input() contact!: IContact;
  @Input() message!: Message;
  //topic = new MessageTopic(1, 'H');
  //message = new Message(1, 1, 1, 'HelloHelloHelloHelloHello', 'date');
  //contact = new Contact(1, 'mail@mail.com', '+79201230000', 'Viktor');
  

  constructor() {}

  ngOnInit() {
    console.log('LOADED DIAGRAM');
  }
}
