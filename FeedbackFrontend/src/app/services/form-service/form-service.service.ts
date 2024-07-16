import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MessageTopic } from '../../models/messagetopic';
import { catchError, throwError } from 'rxjs';
import { NewMessage } from '../../models/newMessage';
import { Contact } from '../../models/contact';
import { IMessage, Message } from '../../models/message';

@Injectable({
  providedIn: 'root'
})
export class FormServiceService {
  //URLы для запросов
  topicsUrl = 'http://localhost:5100/api/messagetopic'; //USING GET
  postMessageUrl = 'http://localhost:5100/api/message'; //USING POST
  //Переменные для хранения сообщений об ошибках. Одна для топиков, другая для поста
  public topicRequestErrorMessage: string = '';
  public postFormRequestErrorMessage: string ='';

  constructor(private http:HttpClient) { }

  //Опции/заголовки хттп
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*',
    }),
  };

  getTopics() {
    /* не теряй контекст!! err => hangle(err)! иначе изменение строки в handle = cannot set/read undefined */
    return this.http.get<MessageTopic[]>(this.topicsUrl, this.httpOptions).pipe(catchError(err => this.handleErrorTopics(err)));
  }

  postMessage(message: NewMessage) {
    return this.http.post<any>(this.postMessageUrl, message).pipe(catchError(err => this.handleErrorPOST(err)));
  }

  //Обработчик ошибок при GET TOPICS
  private handleErrorTopics(error: HttpErrorResponse) {
    if (error.status === 0) {
      console.error('An error occurred duting getting topics.');
      this.topicRequestErrorMessage = "Список тем недоступен. Ошибка соединения с сервером.";
    } else {
      console.error(`Backend returned code ${error.status}, body was: `, error.error);
        this.topicRequestErrorMessage = 'Внутреняя ошибка сервера.';
    }
    //Да, кстати, throwер можно не комментировать, он никогда не выполнится :)
    return [];throwError(() => new Error('Something bad happened; please try again later.'));
  }

  //Обработчик ошибок при POST MESSAGE
  private handleErrorPOST(error: HttpErrorResponse) {
    if (error.status === 0) {
      console.error('An error occurred during posting message to server.', error.error);
      this.postFormRequestErrorMessage = 'Не удалось отправить заявку. Сервер недоступен. Повторите попытку позже.';
      
    } else {
      console.error(`Backend returned code ${error.status}, body was: `, error.error);
        this.postFormRequestErrorMessage = 'Ошибка обработки заявки сервером. Повторите попытку позже.';
    }
    return [];throwError(() => new Error('Something bad happened; please try again later.'));
  }

}
