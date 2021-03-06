import axios from "axios";
import { getField, updateField } from "vuex-map-fields";

export default {
  namespaced: true,
  state() {
    return {
      teams: [],
      team: {
        Id: "",
        Name: "",
        LeagueId: "",
        LeagueName: "",
        Image: "",
        InjuredPlayers: [],
        LoginUsername: "",
        Email: "",
        IsActive: false,
        LoginPassword: "",
        FirstName: "",
        LastName: "",
        DateOfBirth: null,
      },
      positionsData: [],
    };
  },

  getters: {
    getTeams(state) {
      return state.teams;
    },
    getPositionsData(state) {
      return state.positionsData;
    },
    getField,
  },
  mutations: {
    SET_TEAMS(state, payload) {
      state.teams = payload;
    },
    RESET_TEAMS(state) {
      state.teams = [];
    },
    SET_TEAM_DETAILS(state, payload) {
      state.team.Id = payload.id;
      state.team.Name = payload.name;
      state.team.LeagueId = payload.leagueId;
      state.team.LeagueName = payload.leagueName;
      state.team.Image = payload.image;
      state.team.InjuredPlayers = payload.injuredPlayers;
      state.team.IsActive = payload.isActive;
    },
    RESET_TEAM_DETAILS(state) {
      state.team.Id = "";
      state.team.Name = "";
      state.team.LeagueId = "";
      state.team.LeagueName = "";
      state.team.Image = "";
      state.team.InjuredPlayers = [];
      state.team.LoginUsername = "";
      state.team.Email = "";
      state.team.IsActive = false;
      state.team.LoginPassword = "";
      state.team.FirstName = "";
      state.team.LastName = "";
      state.team.DateOfBirth = null;
    },
    SET_POSITIONS_DATA(state, payload) {
      state.positionsData = payload;
    },
    updateField,
  },
  actions: {
    setAllTeams({ commit }) {
      axios.get("https://localhost:44369/api/teams").then((response) => {
        commit("SET_TEAMS", response.data);
      });
    },
    addTeam: async ({ state }) => {
      await axios.post("https://localhost:44369/api/teams", state.team);
    },
    editTeamAdmin: async ({ state, dispatch }, selectedLeagueId) => {
      await axios
        .post("https://localhost:44369/api/admins/teamEdit", state.team)
        .then(() => {
          dispatch("setTeamsByLeagueAdmin", selectedLeagueId);
        });
    },
    teamDeleteAdmin: async ({ state, dispatch }, selectedLeagueId) => {
      await axios
        .post("https://localhost:44369/api/admins/teamDeleteAdmin", {
          Id: state.team.Id,
        })
        .then(() => {
          dispatch("setTeamsByLeagueAdmin", selectedLeagueId);
        });
    },
    teamImageDeleteAdmin: async ({ state }) => {
      await axios.post(
        "https://localhost:44369/api/admins/teamImageDeleteAdmin",
        {
          Id: state.team.Id,
        }
      );
    },
    setTeamsByLeague({ commit }, leagueId) {
      axios
        .get("https://localhost:44369/api/teams/getTeamsByLeague", {
          params: { leagueId: leagueId },
        })
        .then((response) => {
          commit("SET_TEAMS", response.data);
        });
    },
    setTeamsByLeagueAdmin({ commit }, leagueId) {
      axios
        .get("https://localhost:44369/api/admins/getTeamsList", {
          params: { leagueId: leagueId },
        })
        .then((response) => {
          commit("SET_TEAMS", response.data);
        });
    },
    setTeamDetails: async ({ commit }, teamId) => {
      await axios
        .get("https://localhost:44369/api/teams/details", {
          params: { id: teamId },
        })
        .then(function(response) {
          commit("SET_TEAM_DETAILS", response.data);
        });
    },
    setPositionsInsideOfTeam: async ({ commit }, teamId) => {
      await axios
        .get("https://localhost:44369/api/teams/getPositionsInsideOfTeam", {
          params: { teamId: teamId },
        })
        .then(function(response) {
          commit("SET_POSITIONS_DATA", response.data);
        });
    },
    setPositionsToLookup: () => {
      return new Promise((resolve, reject) =>
        axios
          .get("https://localhost:44369/api/teams/getPositionsToLookup")
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
    setTeamsToLookup: () => {
      return new Promise((resolve, reject) =>
        axios.get("https://localhost:44369/api/teams").then(
          (response) => {
            resolve(response);
          },
          (error) => {
            reject(error);
          }
        )
      );
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
  },
};
