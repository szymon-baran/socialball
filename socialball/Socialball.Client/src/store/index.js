import { createStore } from "vuex";

import playersModule from "./modules/players/index";
import teamsModule from "./modules/teams/index";
import matchesModule from "./modules/matches/index";
import authenticationModule from "./modules/authentication/index";
import messagesModule from "./modules/messages/index";
import jobAdvertisementsModule from "./modules/jobAdvertisements/index";
import playerTransferOffersModule from "./modules/playerTransferOffers/index";

const store = createStore({
  modules: {
    players: playersModule,
    teams: teamsModule,
    matches: matchesModule,
    authentication: authenticationModule,
    messages: messagesModule,
    jobAdvertisements: jobAdvertisementsModule,
    playerTransferOffers: playerTransferOffersModule,
  },
});

export default store;
