import { GiftClient, Gift, GiftInput } from "./secretsanta-client"

export class ListGift
{
    async getAllGifts(): Promise<Gift[]> {
        var client = new GiftClient();
        var gifts = await client.getAll();
        return gifts;
    }

    async fakeGiftLists(): Promise<Gift[]> {
        var client = new GiftClient();
        var gifts = await this.getAllGifts();
        (gifts).forEach( gift => {
            client.delete(gift.id);
        });
        let fakeGifts : Gift[] = [
                                    new Gift({
                                                id: 1, 
                                                title: "Princess Bride: Movie", 
                                                description: "pretty good", 
                                                url: "www.movie.com", 
                                                userId: 1
                                            }), 
                                    new Gift({
                                                id: 2, 
                                                title: "Princess Bride: Book", 
                                                description: "pretty good", 
                                                url: "www.book.com", 
                                                userId: 1
                                            }),
                                    new Gift({
                                                id: 3, 
                                                title: "Six Fingered Glove", 
                                                description: "It has six fingers", 
                                                url: "www.specialgloves.com",
                                                 userId: 1
                                            }),
                                    new Gift({
                                                id: 4,
                                                title: "Iocane powder",
                                                description: "You can train yourself to" +
                                                              "be immune to this poison",
                                                url: "www.superdeadly.com",
                                                userId: 1
                                            }), 
                                    new Gift({
                                                id: 5,
                                                title: "R.O.U.S.",
                                                description: "It is a rodent of unusual size",
                                                url: "www.fireswamp.com", 
                                                userId: 1
                                            })
                                ];

        return fakeGifts;
    }
}