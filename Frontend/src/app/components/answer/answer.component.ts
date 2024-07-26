import { Component, Input } from '@angular/core';
import { TMessageFull } from '../../models/message';
import { CommonModule } from '@angular/common';
//import { stringWithMaxLen } from '../../helpers/truncateString';

@Component({
  selector: 'app-answer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './answer.component.svg',
  styleUrl: './answer.component.css'
})



export class AnswerComponent {
  @Input() postedMessage?: TMessageFull; //Форме передаем объект MessagFull

  constructor() {}

  //Усечение строки
  stringWithMaxLen(s: string | undefined, len: number) {
    s ??= "";
    if (s.length > len)
      return s.slice(0, len) + '...';
    return s;
  };

  ngAfterViewInit() {
    console.log('Rendering DB form with data: ', this.postedMessage);
  }
}
