import axios from "axios";
import { getField, updateField } from "vuex-map-fields";
import authHeader from "../../../services/auth-header";

export default {
  namespaced: true,
  state() {
    return {
      matches: [],
      match: {
        Id: "",
        HomeTeamId: "",
        HomeTeam: {},
        AwayTeamId: "",
        AwayTeam: {},
        Stadium: "",
        DateTime: null,
        MatchEvents: [],
        HomeMatchGoalkeeper: {},
        HomeMatchDefenders: {},
        HomeMatchMidfielders: {},
        HomeMatchStrikers: {},
        AwayMatchGoalkeeper: {},
        AwayMatchDefenders: {},
        AwayMatchMidfielders: {},
        AwayMatchStrikers: {},
        IsConfirmed: false,
        IsUnconfirmedByYourTeam: false,
        AddedByTeamId: "",
        MatchType: null,
      },
    };
  },
  getters: {
    getMatches(state) {
      return state.matches;
    },
    getField,
  },
  mutations: {
    SET_MATCHES(state, payload) {
      state.matches = payload;
      state.matches.forEach((match) => {
        let homeGoals = 0;
        let awayGoals = 0;
        match.matchPlayers.forEach((matchPlayer) => {
          matchPlayer.matchEvents.forEach((event) => {
            // Filtrowanie tylko goli
            if (event.matchEventType === 1) {
              if (matchPlayer.teamId === match.homeTeamId) {
                homeGoals++;
              } else if (matchPlayer.teamId === match.awayTeamId) {
                awayGoals++;
              }
            }
          });
        });
        match.result = homeGoals + ":" + awayGoals;
      });
    },
    RESET_MATCHES(state) {
      state.matches = [];
    },
    SET_MATCH_DETAILS(state, payload) {
      state.match.Id = payload.id;
      state.match.HomeTeamId = payload.homeTeamId;
      state.match.HomeTeam = payload.homeTeam;
      state.match.AwayTeamId = payload.awayTeamId;
      state.match.AwayTeam = payload.awayTeam;
      state.match.Stadium = payload.stadium;
      state.match.DateTime = payload.dateTime;
      state.match.MatchEvents = payload.matchEvents.sort(
        (x, y) => x.minute - y.minute
      );
      state.match.HomeMatchGoalkeeper = payload.homeMatchGoalkeeper;
      state.match.HomeMatchDefenders = payload.homeMatchDefenders;
      state.match.HomeMatchMidfielders = payload.homeMatchMidfielders;
      state.match.HomeMatchStrikers = payload.homeMatchStrikers;
      state.match.AwayMatchGoalkeeper = payload.awayMatchGoalkeeper;
      state.match.AwayMatchDefenders = payload.awayMatchDefenders;
      state.match.AwayMatchMidfielders = payload.awayMatchMidfielders;
      state.match.AwayMatchStrikers = payload.awayMatchStrikers;
      state.match.IsConfirmed = payload.isConfirmed;
      state.match.IsUnconfirmedByYourTeam = payload.isUnconfirmedByYourTeam;
      state.match.AddedByTeamId = payload.addedByTeamId;
      state.match.MatchType = payload.matchType;
    },
    RESET_MATCH_FORM(state) {
      state.match.Id = "";
      state.match.HomeTeamId = "";
      state.match.HomeTeam = {};
      state.match.AwayTeamId = "";
      state.match.AwayTeam = {};
      state.match.Stadium = "";
      state.match.DateTime = null;
      state.match.MatchEvents = [];
      state.match.HomeMatchGoalkeeper = {};
      state.match.HomeMatchDefenders = [];
      state.match.HomeMatchMidfielders = [];
      state.match.HomeMatchStrikers = [];
      state.match.AwayMatchGoalkeeper = {};
      state.match.AwayMatchDefenders = [];
      state.match.AwayMatchMidfielders = [];
      state.match.AwayMatchStrikers = [];
      state.match.IsConfirmed = false;
      state.match.IsUnconfirmedByYourTeam = false;
      state.match.AddedByTeamId = "";
      state.match.MatchType = null;
    },
    updateField,
  },

  actions: {
    setMatches({ commit }, teamId) {
      return new Promise((resolve) => {
        axios
          .get("https://localhost:44369/api/matches", {
            params: { teamId: teamId },
          })
          .then((response) => {
            commit("SET_MATCHES", response.data);
            resolve(response);
          });
      });
    },
    setUnconfirmedMatches({ commit }, teamId) {
      axios
        .get("https://localhost:44369/api/matches/getUnconfirmedMatches", {
          params: { teamId: teamId },
          headers: authHeader(),
        })
        .then((response) => {
          commit("SET_MATCHES", response.data);
        });
    },
    setMatchDetails: async ({ commit }, matchId) => {
      await axios
        .get("https://localhost:44369/api/matches/details", {
          params: { id: matchId },
        })
        .then(function(response) {
          commit("SET_MATCH_DETAILS", response.data);
        });
    },
    addMatch: async ({ state, dispatch }, teamId) => {
      await axios
        .post("https://localhost:44369/api/matches/addMatch", state.match, {
          headers: authHeader(),
        })
        .then(() => {
          dispatch("setUnconfirmedMatches", teamId);
        });
    },
    getEventTypesToLookup: () => {
      return new Promise((resolve, reject) => {
        axios
          .get("https://localhost:44369/api/matches/getEventTypesToLookup")
          .then(
            (response) => {
              resolve(response);
            },
            (error) => {
              reject(error);
            }
          );
      });
    },
    getPenaltyTypesToLookup: () => {
      return new Promise((resolve, reject) => {
        axios
          .get("https://localhost:44369/api/matches/getPenaltyTypesToLookup")
          .then(
            (response) => {
              resolve(response);
            },
            (error) => {
              reject(error);
            }
          );
      });
    },
    getMatchTypesLookup: () => {
      return new Promise((resolve, reject) => {
        axios
          .get("https://localhost:44369/api/matches/getMatchTypesToLookup")
          .then(
            (response) => {
              resolve(response);
            },
            (error) => {
              reject(error);
            }
          );
      });
    },
    // eslint-disable-next-line no-unused-vars
    getPlayersByTeam: ({ commit }, teamId) => {
      return new Promise((resolve, reject) => {
        axios
          .get("https://localhost:44369/api/matches/getPlayersByTeam", {
            params: {
              teamId: teamId,
            },
          })
          .then(
            (response) => {
              resolve(response);
            },
            (error) => {
              reject(error);
            }
          );
      });
    },
    sendMatchAnswer: async (
      { state, dispatch },
      { isAccepted: isAccepted, teamId: teamId }
    ) => {
      let wrapper = {
        Id: state.match.Id,
        IsAccepted: isAccepted,
        UserTeamId: teamId,
      };
      await axios
        .post("https://localhost:44369/api/matches/sendMatchAnswer", wrapper, {
          headers: authHeader(),
        })
        .then(() => {
          dispatch("setUnconfirmedMatches", teamId);
        });
    },
  },
};
