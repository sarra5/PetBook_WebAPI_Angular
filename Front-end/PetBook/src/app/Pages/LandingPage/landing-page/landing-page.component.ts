import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../../Core/Header/header/header.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-landing-page',
  standalone: true,
  imports: [HeaderComponent,RouterLink],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.css'
})
export class LandingPageComponent implements OnInit {
  images = [
    'url(../../../../assets/Images/Heroimage1.jpg)', 
    'url(../../../../assets/Images/Heroimage3.jpg)',  
    'url(../../../../assets/Images/Heroimage4.avif)',  
    'url(../../../../assets/Images/Heroimage2.jpg)', 
    'url(../../../../assets/Images/Heroimage5.jpg)',  
];
  
currentIndex = 0;

ngOnInit(): void {
  this.changeBackground();
  setInterval(() => this.changeBackground(), 4000);
}
changeBackground(): void {
  const slideshow = document.getElementById('HeroPhoto');
  if (slideshow) {
    setTimeout(() => {
      slideshow.style.backgroundImage = this.images[this.currentIndex];
      this.currentIndex = (this.currentIndex + 1) % this.images.length;
    }, 500); 
  }
}
}

