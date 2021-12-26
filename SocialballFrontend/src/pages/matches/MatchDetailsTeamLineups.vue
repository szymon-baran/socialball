<template>
  <div class="row mt-4 pitch">
    <h4 class="home-header">{{ match.HomeTeam.name }}</h4>
    <div class="container">
      <div class="col text-center">
        <div class="row mt-3">
          <div class="col circle-center">
            <router-link
              :to="{
                name: 'playerDetails',
                params: { id: match.HomeMatchGoalkeeper.playerId },
              }"
            >
              <div class="player home-gk-color">
                {{ match.HomeMatchGoalkeeper.number }}
              </div>
              <span class="text-player">
                {{ match.HomeMatchGoalkeeper.lastName }}
              </span>
            </router-link>
          </div>
        </div>
        <div class="row mt-4">
          <div
            class="col circle-center"
            v-for="defender in match.HomeMatchDefenders"
            v-bind:key="defender"
          >
            <router-link
              :to="{ name: 'playerDetails', params: { id: defender.playerId } }"
            >
              <div class="player home-color">
                {{ defender.number }}
              </div>
              <span class="text-player">
                {{ defender.lastName }}
              </span>
            </router-link>
          </div>
        </div>
        <div class="row mt-4">
          <div
            class="col circle-center"
            v-for="midfielder in match.HomeMatchMidfielders"
            v-bind:key="midfielder"
          >
            <router-link
              :to="{
                name: 'playerDetails',
                params: { id: midfielder.playerId },
              }"
            >
              <div class="player home-color">
                {{ midfielder.number }}
              </div>
              <span class="text-player">
                {{ midfielder.lastName }}
              </span>
            </router-link>
          </div>
        </div>
        <div class="row mt-4">
          <div
            class="col circle-center"
            v-for="striker in match.HomeMatchStrikers"
            v-bind:key="striker"
          >
            <router-link
              :to="{ name: 'playerDetails', params: { id: striker.playerId } }"
            >
              <div class="player home-color">
                {{ striker.number }}
              </div>
              <span class="text-player">
                {{ striker.lastName }}
              </span>
            </router-link>
          </div>
        </div>
        <div class="mt-3">
          &nbsp;
        </div>
        <div class="row mt-4">
          <div
            class="col circle-center"
            v-for="striker in match.AwayMatchStrikers"
            v-bind:key="striker"
          >
            <router-link
              :to="{ name: 'playerDetails', params: { id: striker.playerId } }"
            >
              <div class="player away-color">
                {{ striker.number }}
              </div>
              <span class="text-player">
                {{ striker.lastName }}
              </span>
            </router-link>
          </div>
        </div>
        <div class="row mt-4">
          <div
            class="col circle-center"
            v-for="midfielder in match.AwayMatchMidfielders"
            v-bind:key="midfielder"
          >
            <router-link
              :to="{
                name: 'playerDetails',
                params: { id: midfielder.playerId },
              }"
            >
              <div class="player away-color">
                {{ midfielder.number }}
              </div>
              <span class="text-player">
                {{ midfielder.lastName }}
              </span>
            </router-link>
          </div>
        </div>
        <div class="row mt-4">
          <div
            class="col circle-center"
            v-for="defender in match.AwayMatchDefenders"
            v-bind:key="defender"
          >
            <router-link
              :to="{ name: 'playerDetails', params: { id: defender.playerId } }"
            >
              <div class="player away-color">
                {{ defender.number }}
              </div>
              <span class="text-player">
                {{ defender.lastName }}
              </span>
            </router-link>
          </div>
        </div>
        <div class="row mt-4 mb-3">
          <div class="col circle-center">
            <router-link
              :to="{
                name: 'playerDetails',
                params: { id: match.AwayMatchGoalkeeper.playerId },
              }"
            >
              <div class="player away-gk-color">
                {{ match.AwayMatchGoalkeeper.number }}
              </div>
              <span class="text-player">
                {{ match.AwayMatchGoalkeeper.lastName }}
              </span>
            </router-link>
          </div>
        </div>
      </div>
    </div>
    <h4 class="away-header">{{ match.AwayTeam.name }}</h4>
  </div>
</template>

<script>
import { mapState } from "vuex";

export default {
  name: "MatchDetailsTeamLineups",
  computed: {
    ...mapState("matches", ["match"]),
  },
};
</script>

<style scoped>
.home-header {
  margin: 0 0 1.5vh 0;
  font-weight: bold;
  font-size: 150%;
  letter-spacing: 0.06em;
}
.away-header {
  margin: 1.5vh 0 0 0;
  font-weight: bold;
  font-size: 150%;
  letter-spacing: 0.06em;
}
.pitch {
  background: #1d8649;
  width: 30vw;
  height: 100%;
  border-radius: 5px;
  padding: 20px;
  margin: 0px;
  display: inline-block;
}
.pitch .container {
  width: 95%;
  height: 99%;
  border: 2px solid #eee;
  padding: 0px;
}
a {
  text-decoration: none;
}
a:hover {
  color: #cccccc;
  text-decoration: none;
}
.player {
  text-align: center;
  width: 2vw;
  height: 2vw;
  font-weight: bold;
  font-size: 120%;
  border: 2px solid #2f363c;
  padding: 8px;
  border-radius: 50%;
}
.home-color {
  background: #0066ff;
}
.home-gk-color {
  background: #013179;
}
.away-color {
  background: #ff0000;
}
.away-gk-color {
  background: #880000;
}
.text-player {
  background: #2f363c;
  padding: 4px 6px;
  border-radius: 20px;
}
.circle-center {
  text-align: -webkit-center;
  cursor: pointer;
}
</style>
