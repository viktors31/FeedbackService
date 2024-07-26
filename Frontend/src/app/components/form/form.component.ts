import { Component } from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup, Validators, AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import { TMessageTopics } from '../../models/messagetopic';
import { JsonPipe, NgFor, NgIf } from '@angular/common';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { FormAPIService } from '../../services/form-api/form-api.service';
import { AnswerComponent } from "../answer/answer.component";
import { IMessageFull, TMessageDTO, TMessageFull } from '../../models/message';
import { RecaptchaErrorParameters, RecaptchaModule, RecaptchaFormsModule } from 'ng-recaptcha';

@Component({
  selector: 'app-form',
  standalone: true,
  imports: [ReactiveFormsModule, NgFor, NgIf, NgxMaskDirective, NgxMaskPipe, JsonPipe, AnswerComponent, RecaptchaModule],
  templateUrl: './form.component.html',
  styleUrl: './form.component.css'
})



export class FormComponent {

  public resolved(captchaResponse: string) {
    console.log(`Resolved captcha with response: ${captchaResponse}`);
    this.postMessageForm.patchValue({captcha: captchaResponse});
  }

  public onError(errorDetails: RecaptchaErrorParameters): void {
    console.log(`reCAPTCHA error encountered; details:`, errorDetails);
  }

  //https://stackoverflow.com/questions/42927170/angular2-object-cannot-set-property-of-undefined
  postedMessage?: TMessageFull; //Ответ сервера, полное сообщение с topic и contact info
  topics?: TMessageTopics; //Массив с темами сообщений
  formData?: TMessageDTO; //Данные для создания/отправки нового сообщения

  postMessageForm: FormGroup; //Реактивная форма
  capchaValue: string = ''; //Значение капчи (стринги, так как число пустое сделать не варик)
  success: boolean = false; //Хранит успех отправки, нужно чтобы вывести сообщение (костыль)

  //Паттерн для символа (в маску ввода имени) (только ру/англ буквы)
  nameSymbolPattern = {'N': {pattern: new RegExp('[а-яА-Яa-zA-Z]') }};

  constructor(public formApi:FormAPIService) {
  //Создаем реактивную форму
  this.postMessageForm = new FormGroup({
    contactName : new FormControl('',[Validators.required]),
    contactMail : new FormControl('', [Validators.required, Validators.pattern(/^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/)]),
    contactPhone : new FormControl('', [Validators.required]),
    topicId : new FormControl(0, [Validators.required, this.existTopicValidator()]),
    messageText : new FormControl('', [Validators.required, Validators.maxLength(1024)]),
    captcha : new FormControl(null, [Validators.required]),
  });
  }

  //Свой валидатор для топиков (выбранный id из списка должен быть в массиве с топиками)
  existTopicValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const isexist = this.topics?.find(topicId => topicId.id == control.value) != undefined ? true : false;
      return isexist ? null : { existTopic: { value: control.value } };
    };
  }

  //При инициализации
  ngOnInit() {
    this.formApi.getTopics().subscribe(response => {
      this.topics = response; //Подгрузить топики с сервера
      console.log('Available topics: ', this.topics); //Кидаю в лог
    }); 
    
    //this.topics = [{id: 1, name: 'hello'}]; //заглушка-пробник
  }

  //Отправка формы
  onSubmit() {
    console.log('Sending new message to server: ',this.postMessageForm.value); //В лог кидаю сообщение
    this.formApi.postMessage(this.postMessageForm.value).subscribe((data:IMessageFull) => {
      this.postedMessage = data; //сохраняю ответ в postedMessage
      this.success = true; //Ставим флаг что успешно отправилось
      console.log('New message posted! Answer from server: ', this.postedMessage); //Кидаю ответ в консоль
    });
  }

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
  get captcha() {
    return this.postMessageForm.get('captcha');
  }

}
