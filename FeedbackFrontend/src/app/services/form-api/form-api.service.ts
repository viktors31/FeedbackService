import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TMessageTopics } from '../../models/messagetopic';
import { catchError, Observable, of, retry, throwError } from 'rxjs';
import { IMessageFull, TMessageDTO } from '../../models/message';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})

export class FormAPIService {
  public postedMessage: IMessageFull | undefined;
  //Переменные для хранения сообщений об ошибках. Одна для топиков, другая для поста
  public topicRequestErrorMessage: string = '';
  public postFormRequestErrorMessage: string ='';
  

  constructor(private http:HttpClient) {}

  //Опции/заголовки для запросов HTTP
  httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json', //Контент в формате JSON
      'Access-Control-Allow-Origin': '*', //Отключить CORS
    }),
  };

  getTopics(): Observable<TMessageTopics> {
    return this.http.get<TMessageTopics>(environment.apiUrl + '/messagetopic', this.httpOptions).pipe(retry(3), catchError(err => this.handleErrorTopics(err)));
  }

  postMessage(message: TMessageDTO) : Observable<IMessageFull> {
    return this.http.post<IMessageFull>(environment.apiUrl + '/message', message).pipe(retry(3),catchError(err => this.handleErrorPOST(err)));
  }

  //Обработчик ошибок при GET TOPICS
  private handleErrorTopics(error: HttpErrorResponse) {
    console.error('An error occurred duting getting topics. Response code is ', error.status);
    console.error(error);
    if (error.status === 0) {
      this.topicRequestErrorMessage = "Список тем недоступен. Ошибка соединения с сервером.";
    } else {
        this.topicRequestErrorMessage = 'Внутреняя ошибка сервера.';
    }
    //Вернуть надо Observable, чтобы вернуть пустой нужно написать of()
    return of();
  }

  //Обработчик ошибок при POST MESSAGE
  private handleErrorPOST(error: HttpErrorResponse) {
    console.error('An error occurred during posting message to server. Response code is', error.error);
    console.error(error);
    if (error.status === 0) {
      this.postFormRequestErrorMessage = 'Не удалось отправить заявку. Сервер недоступен. Повторите попытку позже.';
    } else {
        this.postFormRequestErrorMessage = 'Ошибка обработки заявки сервером. Повторите попытку позже.';
    }
    return of();
  }

}
