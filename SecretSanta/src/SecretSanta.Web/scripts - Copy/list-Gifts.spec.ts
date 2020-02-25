import { ListGift } from "./list-Gifts"
import { expect } from "chai"
import 'mocha';
import { IGiftClient, Gift, GiftInput } from "./secretsanta-client";
import { SampleGifts } from "./SampleGifts";
import { ResolvePlugin } from "webpack";

describe('getAllGifts', () => {
    it('should return all gifts', async () => {
        const display = new ListGift(new MockGiftClient());
        const actual = await display.getAllGifts();
        expect(actual.length).to.equal(5);
    })
});

describe('generateGiftList', () => {
    it('delete the 5 existing gifts and add 5 gifts', async () =>{
        let mockGiftClient = new MockGiftClient();
        const display = new ListGift(mockGiftClient);
        await display.generateGiftList();
        expect(mockGiftClient.timesDeleteCalled).to.equal(5);
        expect(mockGiftClient.timesPostCalled).to.equal(5);
    })
})

class MockGiftClient implements IGiftClient{
    timesPostCalled : number = 0;
    timesDeleteCalled : number = 0;
    getAll(): Promise<import("./secretsanta-client").Gift[]> {
        return new Promise( resolve => {
            resolve(SampleGifts());
        });
    }

    post(entity: import("./secretsanta-client").GiftInput): Promise<import("./secretsanta-client").Gift> {
        this.timesPostCalled++;
        let gift : Gift = new Gift({id: this.timesPostCalled,
                                    title: entity.title, 
                                    description: entity.description, 
                                    url: entity.url , 
                                    userId: entity.userId });
        
        return new Promise( resolve => {
            resolve(gift);
        });
    }

    get(id: number): Promise<import("./secretsanta-client").Gift> {
        throw new Error("Method not implemented.");
    }
    put(id: number, value: import("./secretsanta-client").GiftInput): Promise<import("./secretsanta-client").Gift> {
        throw new Error("Method not implemented.");
    }
    delete(id: number): Promise<void> {
        this.timesDeleteCalled++;
        return;
    }


}