import axios from "axios";
import { getField, updateField } from "vuex-map-fields";
import authHeader from "../../../services/auth-header";

export default {
  namespaced: true,
  state() {
    return {
      playerTransferOffers: [],
      playerTransferOffer: {
        Id: "",
        PlayerId: "",
        FromTeamId: "",
        ToTeamId: "",
        Content: "",
        TransferFee: null,
        IsAcceptedByPlayer: false,
        IsAcceptedByOtherTeam: false,
        PlayerName: "",
        FromTeamName: "",
        ToTeamName: "",
      },
    };
  },

  getters: {
    getPlayerTransferOffers(state) {
      return state.playerTransferOffers;
    },
    getField,
  },
  mutations: {
    SET_PLAYER_TRANSFER_OFFERS(state, payload) {
      state.playerTransferOffers = payload;
    },
    RESET_PLAYER_TRANSFER_OFFERS(state) {
      state.playerTransferOffers = [];
    },
    SET_PLAYER_TRANSFER_OFFER_DETAILS(state, payload) {
      state.playerTransferOffer.Id = payload.id;
      state.playerTransferOffer.PlayerId = payload.playerId;
      state.playerTransferOffer.FromTeamId = payload.fromTeamId;
      state.playerTransferOffer.ToTeamId = payload.toTeamId;
      state.playerTransferOffer.Content = payload.content;
      state.playerTransferOffer.TransferFee = payload.transferFee;
      state.playerTransferOffer.IsAcceptedByPlayer = payload.isAcceptedByPlayer;
      state.playerTransferOffer.IsAcceptedByOtherTeam =
        payload.isAcceptedByOtherTeam;
      state.playerTransferOffer.PlayerName = payload.playerName;
      state.playerTransferOffer.FromTeamName = payload.fromTeamName;
      state.playerTransferOffer.ToTeamName = payload.toTeamName;
    },
    RESET_PLAYER_TRANSFER_OFFER_FORM(state) {
      state.playerTransferOffer.Id = "";
      state.playerTransferOffer.PlayerId = "";
      state.playerTransferOffer.FromTeamId = "";
      state.playerTransferOffer.ToTeamId = "";
      state.playerTransferOffer.Content = "";
      state.playerTransferOffer.TransferFee = null;
      state.playerTransferOffer.IsAcceptedByPlayer = false;
      state.playerTransferOffer.IsAcceptedByOtherTeam = false;
      state.playerTransferOffer.PlayerName = "";
      state.playerTransferOffer.FromTeamName = "";
      state.playerTransferOffer.ToTeamName = "";
    },
    updateField,
  },

  actions: {
    setToTeamTransferOffers({ commit }, teamId) {
      axios
        .get(
          "https://localhost:44369/api/playerTransferOffers/getTeamTransferOffers",
          {
            params: { teamId: teamId },
          }
        )
        .then((response) => {
          commit("SET_PLAYER_TRANSFER_OFFERS", response.data);
        });
    },
    setFromTeamTransferOffers({ commit }, teamId) {
      axios
        .get(
          "https://localhost:44369/api/playerTransferOffers/getFromTeamTransferOffers",
          {
            params: { teamId: teamId },
          }
        )
        .then((response) => {
          commit("SET_PLAYER_TRANSFER_OFFERS", response.data);
        });
    },
    addPlayerTransferOffer: async ({ state, dispatch }) => {
      await axios
        .post(
          "https://localhost:44369/api/playerTransferOffers/addPlayerTransferOffer",
          state.playerTransferOffer,
          {
            headers: authHeader(),
          }
        )
        .then(() => {
          dispatch(
            "setToTeamTransferOffers",
            state.playerTransferOffer.FromTeamId
          );
        });
    },
  },
};
