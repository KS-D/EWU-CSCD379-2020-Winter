import { GiftClient, IGiftClient } from "./secretsanta-client"
import { SampleGifts } from "./SampleGifts";

export class ListGift
{
    giftClient : IGiftClient;

    constructor(giftClient : IGiftClient = new GiftClient()){
        this.giftClient = giftClient;
    };

    async getAllGifts(){
        var gifts = await this.giftClient.getAll();
        return gifts;
    }

    async generateGiftList(){
        var gifts = await this.getAllGifts();
        (gifts).forEach( gift => {
            this.giftClient.delete(gift.id);
        });
        
        let fakeGiftLists = SampleGifts();
        fakeGiftLists.forEach( gift => {
            this.giftClient.post(gift);
        });
    }

    async renderGifts() {
       console.log('hello world');
       var gifts = await this.getAllGifts();
       const itemList = document.getElementById("giftList");
       gifts.forEach( gift => {
           const listItem = document.createElement("li");
           listItem.textContent = `${gift.id}:${gift.title}:${gift.description}:${gift.url}:${gift.url}`
           itemList.append(listItem);
       }) 
    }
}