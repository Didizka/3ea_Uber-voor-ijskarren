import {Component, OnInit, ChangeDetectorRef} from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import {OnRatingChangeEven} from "angular-star-rating";
import {Review} from "../../Models/review";
import {Driver} from "../../Models/driver";
import {Customer} from "../../Models/customer";
import {Storage} from "@ionic/storage";
import {ReviewProvider} from "../../providers/review";
import {UserProvider} from "../../providers/user";

@IonicPage()
@Component({
  selector: 'page-review',
  templateUrl: 'review.html',
})
export class ReviewPage implements OnInit{
  review: Review = {
    review: "",
    rating: 0,
    reviewToEmail: null,
    reviewFromEmail: null,
    reviewerName: null
  };
  reviewTextArray: string[] = ['Very bad ', 'Bad ', 'Average ', 'Good', 'Very good'];
  reviewText = "";
  driver: Driver;
  customerEmail = "";
  constructor(private navCtrl: NavController,
              private navParams: NavParams,
              private ref: ChangeDetectorRef,
              private reviewProvider: ReviewProvider,
              private userProvider: UserProvider) {
  }
  ngOnInit() {
    this.userProvider.getCurrentUser().then(
      user => {
        this.customerEmail = user;
        this.review.reviewFromEmail = this.customerEmail;
      });
    this.driver = this.navParams.get('driver');
    this.review.reviewToEmail = this.driver.email;

  }
  onRatingChange = ($event:OnRatingChangeEven) => {
    this.review.rating = $event.rating;
    this.reviewText = this.reviewTextArray[$event.rating-1];
    this.ref.detectChanges();
    //console.log(this.review);
  };
  onSubmitReview(){
    this.review.reviewToEmail = this.driver.email;
    this.reviewProvider.addReviewToDriver(this.review).subscribe(
      data => {
        if(data == true){
          this.navCtrl.setRoot('ListDriversPage');
        }
      }
    );
  }
  onNotNow(){
    this.navCtrl.setRoot('ListDriversPage');
  }
}
