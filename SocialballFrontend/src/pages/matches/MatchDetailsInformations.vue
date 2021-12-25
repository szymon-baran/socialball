<template>
  <div class="mt-4">
    <div class="row">
      <div class="col">
        {{ match.Stadium }}
      </div>
    </div>
    <div class="row">
      <div class="col">
        {{ getFormattedDateTime }}
      </div>
    </div>
    <div class="row" v-if="matchTypes && match">
      <div class="col">
        <span v-if="matchTypes[match.MatchType]">{{ matchTypes[match.MatchType].name }}</span>
      </div>
    </div>
    <div class="line"></div>
    <div v-for="event in match.MatchEvents" v-bind:key="event.id">
      <div class="row">
        <div class="col" v-if="event.teamId === match.AwayTeamId" />
        <div class="col" v-if="event.matchEventType === 1">
          <i class="fas fa-futbol ico main-color"></i>
          {{ event.minute }}'
          {{ event.lastName }}
          <div v-if="event.assistPlayerLastName">
            <i class="far fa-handshake ico main-color"></i>
            {{ event.assistPlayerLastName }}
          </div>
        </div>
        <div class="col" v-if="event.matchEventType === 2">
          <span v-if="event.penaltyType === 0">
            <i class="fas fa-exclamation ico main-color"></i>
            {{ event.minute }}' {{ event.lastName }}
          </span>
          <span v-if="event.penaltyType === 1">
            <i class="fas fa-square ico yellow-color"></i>
            {{ event.minute }}' {{ event.lastName }}
          </span>
          <span v-if="event.penaltyType === 2">
            <i class="fas fa-square ico red-color"></i>
            {{ event.minute }}'
            {{ event.lastName }}
          </span>
        </div>
        <div class="col" v-if="event.teamId === match.HomeTeamId" />
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapState } from "vuex";

export default {
  name: "MatchDetailsInformations",
  data() {
    return {
      matchTypes: [],
    };
  },
  computed: {
    ...mapState("matches", ["match"]),
    getFormattedDateTime() {
      var tmpDate = new Date(this.match.DateTime);
      return tmpDate.toLocaleString();
    },
  },
  methods: {
    ...mapActions({
      getMatchTypesLookup: "matches/getMatchTypesLookup",
    }),
  },
  mounted() {
    this.getMatchTypesLookup().then((response) => {
      this.matchTypes = response.data;
    });
  },
};
</script>
