import { Component } from '@angular/core';
import { FormComponent } from "./components/form/form.component";
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { AnswerComponent } from "./components/answer/answer.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormComponent, NgxMaskDirective, NgxMaskPipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'FeedbackFrontend';
}
