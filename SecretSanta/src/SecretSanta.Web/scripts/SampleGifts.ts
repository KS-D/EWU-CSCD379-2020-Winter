import { Gift } from "./secretsanta-client";


export const SampleGifts = () => {
    return [
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
                        description: "pretty good, but you have to read it", 
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
}