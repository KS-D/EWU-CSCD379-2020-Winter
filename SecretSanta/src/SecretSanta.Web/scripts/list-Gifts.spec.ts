import { ListGift } from "./list-Gifts"

import { expect } from "chai"
import chai from "chai";
import chaiHttp from "chai-http"
import 'mocha';

describe('GetAllGifts', () => {
    it('should return all gifts', async () =>{
        const display = new ListGift();
        const actual = await display.getAllGifts();
        expect(actual.length).to.equal(0);
    })
});

describe('fakeGiftLists', () => {
    it('should return all gifts', async () =>{
        const display = new ListGift();
        const actual = await display.fakeGiftLists();
        expect(actual.length).to.equal(5);
    })
})