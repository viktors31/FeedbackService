import { Component } from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup, Validators, AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import { IMessageTopic, MessageTopic } from '../../models/messagetopic';
import { NewMessage } from '../../models/newMessage';
import { JsonPipe, NgFor, NgIf } from '@angular/common';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { FormServiceService } from '../../services/form-service/form-service.service';
import { delay, Observable } from 'rxjs';
import { AnswerComponent } from "../answer/answer.component";
import { IMessage, Message, Message2 } from '../../models/message';
import { Contact } from '../../models/contact';
//import { ImessageDbFrame } from '../../models/messageDbFrame';

//Определим тип, без него ангуляр ругается :(
type messageTopics = Array<MessageTopic>
interface ImessageDbFrame {
  topic: MessageTopic,
  message: Message,
  contact: Contact,
}

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [ReactiveFormsModule, NgFor, NgIf, NgxMaskDirective, NgxMaskPipe, JsonPipe, AnswerComponent],
  templateUrl: './form.component.html',
  styleUrl: './form.component.css'
})



export class FormComponent {
  //Надо объявлять интерфейс и пустой объект вот так правильно "объкты-переменные" создавать!!!
  //https://stackoverflow.com/questions/42927170/angular2-object-cannot-set-property-of-undefined
  //postAnswer: IMessage = {};
  postAnswer: ImessageDbFrame | undefined;
  
  //Подмена топиков (заместо реальноного запроса)
  /*topics: messageTopics = [
    {id: 1, name: 'T1'},
    {id: 2, name: 'T2'},
  ];*/
  topics: messageTopics = [];

  postMessageForm: FormGroup; //Реактивная форма
  formData = new NewMessage('','' , '', 0, ''); //Данные сюда
  capchaValue: string = ''; //Значение капчи (стринги, так как число пустое сделать не варик)
  success: boolean = false; //Хранит успех отправки, нужно чтобы вывести сообщение (костыль)

  //Паттерн для символа (в маску ввода имени)
  nameSymbolPattern = {'N': {pattern: new RegExp('[а-яА-Яa-zA-Z]') }};

 //Getterы для проброса ошибок в HTML
 get contactName() {
  return this.postMessageForm.get('contactName');
 }
 get contactMail() {
  return this.postMessageForm.get('contactMail');
 }
 get contactPhone() {
  return this.postMessageForm.get('contactPhone');
 }
 get topicId() {
  return this.postMessageForm.get('topicId');
 }
 get messageText() {
  return this.postMessageForm.get('messageText');
 }
 get capcha() {
  return this.postMessageForm.get('capcha');
 }

  constructor(public formApi:FormServiceService) {
  //Создание формы
  this.postMessageForm = new FormGroup({
    contactName : new FormControl(this.formData.contactName, [Validators.required]),
    contactMail : new FormControl(this.formData.contactMail, [Validators.required, Validators.pattern('[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$')]),
    contactPhone : new FormControl(this.formData.contactPhone, [Validators.required]),
    messageTopic : new FormControl(this.formData.topicId, [this.existTopicValidator()]),
    messageText : new FormControl(this.formData.messageText, [Validators.required, Validators.maxLength(1024)]),
    capcha : new FormControl(this.capchaValue, Validators.required),
  });
  //this.postAnswer.topic = {id: 0, name: 'no_name'};


  }
  //Валидатор для топиков (если выбран id не из массива - такого топика не существует)
  existTopicValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const isexist = this.topics.find(topicId => topicId.id == control.value) != undefined ? true : false;
      return isexist ? null : { existTopic: { value: control.value } };
    };
  }

  ngOnInit() {
    this.formApi.getTopics().subscribe(response => {this.topics = response;});
    console.log('tops ', this.topics);
  }

  onSubmit() {
    //Как достать Json response..? 
    console.log('Sending... ',this.formData);

    console.log('ANSWER FROM SERVER BEFORE ', this.postAnswer);
    //this.formApi.postMessage(this.formData).subscribe((data:IMessage) => this.postAnswer = {id:7777, topicId : data.topicId, contactId : data.contactId, messageText : data.messageText, postDate: data.postDate});
    this.formApi.postMessage(this.formData).subscribe((data:ImessageDbFrame) => {this.postAnswer = data; this.success = true;});
    //this.formApi.postMessage(this.formData).subscribe((data:IMessage) => {this.postAnswer = data; console.log('getted ', this.postAnswer)});

    //this.formApi.getTopics().subscribe(response => {this.postAnswer = {id:777, topicId : 1, contactId : 1, messageText : 'ppppp', postDate: undefined};});
    //this.postAnswer = {id:777, topicId : 1, contactId : 1, messageText : 'ppppp', postDate: undefined};
    console.log('ANSWER FROM SERVER ', this.postAnswer);
    //this.success = true;
  }

}
