import { ListGift } from "./list-Gifts"
import { expect } from "chai"
import 'mocha';
import { IGiftClient, Gift } from "./secretsanta-client";

describe('GetAllGifts', () => {
    it('should return all gifts', async () =>{
        const display = new ListGift(new MockGiftClient());
        const actual = await display.getAllGifts();
        expect(actual.length).to.equal(5);
    })
});

describe('fakeGiftLists', () => {
    it('should return all gifts', async () =>{
        const display = new ListGift(new MockGiftClient());
        const actual = await display.fakeGiftLists();
        expect(actual.length).to.equal(5);
    })
})

class MockGiftClient implements IGiftClient{
    getAll(): Promise<import("./secretsanta-client").Gift[]> {
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

        return new Promise( resolve => {
            resolve(fakeGifts);
        });
    }    
    post(entity: import("./secretsanta-client").GiftInput): Promise<import("./secretsanta-client").Gift> {
        throw new Error("Method not implemented.");
    }
    get(id: number): Promise<import("./secretsanta-client").Gift> {
        throw new Error("Method not implemented.");
    }
    put(id: number, value: import("./secretsanta-client").GiftInput): Promise<import("./secretsanta-client").Gift> {
        throw new Error("Method not implemented.");
    }
    delete(id: number): Promise<void> {
        return;
    }


}