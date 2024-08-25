import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-input-section',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './input-section.component.html',
  styleUrl: './input-section.component.css'
})
export class InputSectionComponent {
  @Input() typeOfInput = "text"
  @Input() LabelType = "text"
  @Input() defaultValue: string = '';

  inputValue: string = '';

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['defaultValue']) {
      this.inputValue = this.defaultValue || '';
    }
  }

  @Output() inputValueChange: EventEmitter<string> = new EventEmitter<string>();

  onInputValueChange(value: string) {
    this.inputValue = value;
    this.inputValueChange.emit(this.inputValue);
  }

}
