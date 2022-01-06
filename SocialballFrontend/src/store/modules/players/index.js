import axios from "axios";
import { getField, updateField } from "vuex-map-fields";
import authHeader from "../../../services/auth-header";

export default {
  namespaced: true,
  state() {
    return {
      players: [],
      player: {
        Id: "",
        FirstName: "",
        LastName: "",
        DateOfBirth: null,
        IsInjuredUntil: null,
        Email: "",
        Position: null,
        Earnings: null,
        TeamId: "",
        Citizenship: "",
        LoginUsername: "",
        LoginPassword: "",
        Team: {},
        Image: null,
        Goals: [],
        Assists: [],
        CurrentYearGoalsToChart: [],
        TeamName: "",
        UserId: "",
        MatchCount: null,
      },
    };
  },
  getters: {
    getPlayers(state) {
      return state.players;
    },
    getPlayerDetails(state) {
      return state.player;
    },
    getField,
  },
  mutations: {
    SET_PLAYERS(state, payload) {
      state.players = payload;
    },
    RESET_PLAYERS(state) {
      state.players = [];
    },
    SET_PLAYER_DETAILS(state, payload) {
      state.player.Id = payload.id;
      state.player.FirstName = payload.firstName;
      state.player.LastName = payload.lastName;
      state.player.DateOfBirth = new Date(payload.dateOfBirth);
      state.player.IsInjuredUntil = payload.isInjuredUntil;
      state.player.Email = payload.email;
      state.player.Position = payload.position;
      state.player.Earnings = payload.earnings;
      state.player.TeamId = payload.teamId;
      state.player.Citizenship = payload.citizenship;
      state.player.Team = payload.team;
      state.player.Image = payload.image;
      state.player.Goals = payload.goals;
      state.player.Assists = payload.assists;
      state.player.CurrentYearGoalsToChart = payload.currentYearGoalsToChart;
      state.player.TeamName = payload.teamName;
      state.player.UserId = payload.userId;
      state.player.MatchCount = payload.matchCount;
    },
    RESET_PLAYER_FORM(state) {
      state.player.Id = "";
      state.player.FirstName = "";
      state.player.LastName = "";
      state.player.DateOfBirth = null;
      state.player.IsInjuredUntil = null;
      state.player.Email = "";
      state.player.Position = null;
      state.player.Earnings = null;
      state.player.TeamId = "";
      state.player.Citizenship = "";
      state.player.LoginUsername = "";
      state.player.LoginPassword = "";
      state.player.Team = {};
      state.player.Image = null;
      state.player.MatchEvents = [];
      state.player.Goals = [];
      state.player.Assists = [];
      state.player.CurrentYearGoalsToChart = [];
      state.player.TeamName = "";
      state.player.UserId = "";
      state.player.MatchCount = null;
    },
    updateField,
  },

  actions: {
    setPlayers({ commit }, profileTeamId) {
      axios
        .get("https://localhost:44369/api/players", {
          params: { teamId: profileTeamId },
          headers: authHeader(),
        })
        .then((response) => {
          commit("SET_PLAYERS", response.data);
        });
    },
    setUsersAdmin({ commit }) {
      axios
        .get("https://localhost:44369/api/admins/getUsersList", {
          headers: authHeader(),
        })
        .then((response) => {
          commit("SET_PLAYERS", response.data);
        });
    },
    setLeaguesToLookup: () => {
      return new Promise((resolve, reject) =>
        axios.get("https://localhost:44369/api/teams/getLeaguesToLookup").then(
          (response) => {
            resolve(response);
          },
          (error) => {
            reject(error);
          }
        )
      );
    },
    addPlayer: async ({ state }) => {
      await axios.post("https://localhost:44369/api/players", state.player);
    },
    editPlayer: async ({ state }) => {
      const wrapper = {
        Id: state.player.Id,
        FirstName: state.player.FirstName,
        LastName: state.player.LastName,
        Position: state.player.Position,
        Citizenship: state.player.Citizenship,
        Email: state.player.Email,
        DateOfBirth: state.player.DateOfBirth,
      };
      wrapper.Image = Array.isArray(state.player.Image)
        ? state.player.Image
        : null;
      await axios.post("https://localhost:44369/api/players/edit", wrapper);
    },
    addEditPlayerInjury: async ({ state, dispatch }, teamId) => {
      const wrapper = {
        Id: state.player.Id,
        IsInjuredUntil: state.player.IsInjuredUntil,
      };
      await axios.post(
        "https://localhost:44369/api/players/addEditPlayerInjury",
        wrapper
      );
      dispatch("setPlayers", teamId);
    },
    setPlayerDetails: async ({ commit }, playerId) => {
      await axios
        .get("https://localhost:44369/api/players/details", {
          params: { id: playerId },
        })
        .then(function(response) {
          commit("SET_PLAYER_DETAILS", response.data);
        });
    },
    setUserDataDetails: async ({ commit }, userDataId) => {
      await axios
        .get("https://localhost:44369/api/players/userDataDetails", {
          params: { id: userDataId },
        })
        .then(function(response) {
          commit("SET_PLAYER_DETAILS", response.data);
        });
    },
    getPlayerDetailsByUserId: async ({ commit }, userId) => {
      await axios
        .get("https://localhost:44369/api/players/getPlayerByUserId", {
          params: { userId: userId },
        })
        .then(function(response) {
          commit("SET_PLAYER_DETAILS", response.data);
        });
    },
    getUserDataByUserId: async ({ commit }, userId) => {
      await axios
        .get("https://localhost:44369/api/players/getUserDataByUserId", {
          params: { userId: userId },
        })
        .then(function(response) {
          commit("SET_PLAYER_DETAILS", response.data);
        });
    },
    validateUsername: ({ state }) => {
      return new Promise((resolve, reject) => {
        axios
          .get("https://localhost:44369/api/players/isUsernameUnique", {
            params: {
              username: state.player.LoginUsername,
            },
          })
          .then((result) => {
            result.data === true ? resolve() : reject();
          });
      });
    },
    kickPlayerOutOfTeam: async ({ dispatch, state }) => {
      await axios
        .post("https://localhost:44369/api/players/kickPlayerOutOfTeam", {
          Id: state.player.Id,
        })
        .then(() => {
          dispatch("setPlayers", state.player.TeamId);
        });
    },
    setUserTypesToLookup: () => {
      return new Promise((resolve, reject) =>
        axios
          .get("https://localhost:44369/api/players/getUserTypesToLookup")
          .then(
            (response) => {
              resolve(response);
            },
            (error) => {
              reject(error);
            }
          )
      );
    },
    banUserAdmin: async ({ dispatch }, id) => {
      await axios
        .post("https://localhost:44369/api/admins/banUser", {
          Id: id,
        })
        .then(() => {
          dispatch("setUsersAdmin");
        });
    },
    unbanUserAdmin: async ({ dispatch }, id) => {
      await axios
        .post("https://localhost:44369/api/admins/unbanUser", {
          Id: id,
        })
        .then(() => {
          dispatch("setUsersAdmin");
        });
    },
  },
};
