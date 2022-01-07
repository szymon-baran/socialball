<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="520"
      :height="600"
      container=".dx-viewport"
      title="Składanie oferty transferowej"
      :shading="false"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="sendButtonOptionsAccept"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="closeButtonOptions"
      />
      <form>
        <DxValidationGroup :ref="groupRefKey">
          <div class="row">
            <div class="col">
              <label for="toTeamIdSelectBox" class="form-label"
                >Transfer z:</label
              >
              <DxSelectBox
                :dataSource="filteredTeams"
                value-expr="id"
                display-expr="name"
                v-model="ToTeamId"
                id="toTeamIdSelectBox"
                @value-changed="onToTeamChange"
              >
                <DxValidator>
                  <DxRequiredRule message="Wybierz drużynę zawodnika!" />
                </DxValidator>
              </DxSelectBox>
            </div>
            <div class="col">
              <label for="fromTeamIdSelectBox" class="form-label"
                >Transfer do:</label
              >
              <DxSelectBox
                :dataSource="teams"
                value-expr="id"
                display-expr="name"
                v-model="FromTeamId"
                id="fromTeamIdSelectBox"
                :read-only="true"
              >
                <DxValidator>
                  <DxRequiredRule message="Wybierz drużynę!" />
                </DxValidator>
              </DxSelectBox>
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <label for="playerIdSelectBox" class="form-label">Zawodnik</label>
              <DxSelectBox
                :dataSource="players"
                value-expr="id"
                display-expr="name"
                v-model="PlayerId"
                id="playerIdSelectBox"
              >
                <DxValidator>
                  <DxRequiredRule message="Wybierz drużynę!" />
                </DxValidator>
              </DxSelectBox>
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <label for="transferFeeNumberBox" class="form-label"
                >Kwota transferu [PLN]</label
              >
              <DxNumberBox v-model="TransferFee" id="transferFeeNumberBox">
                <DxValidator>
                  <DxRequiredRule message="Podaj kwotę transferu!" />
                </DxValidator>
              </DxNumberBox>
            </div>
            <div class="col">
              <label for="playerEarningsNumberBox" class="form-label"
                >Zarobki zawodnika [PLN/miesiąc]</label
              >
              <DxNumberBox v-model="PlayerEarnings" id="playerEarningsNumberBox" />
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <label for="contentTextArea" class="form-label">Treść</label>
              <DxTextArea
                v-model="Content"
                id="contentTextArea"
                :auto-resize-enabled="true"
                :max-height="180"
              >
                <DxValidator>
                  <DxRequiredRule message="Wprowadź treść oferty!" />
                </DxValidator>
              </DxTextArea>
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <DxValidationSummary />
            </div>
          </div>
        </DxValidationGroup>
      </form>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import DxTextArea from "devextreme-vue/text-area";
import { mapActions, mapMutations, mapGetters } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "playerTransferOffers/getField",
  mutationType: "playerTransferOffers/updateField",
});
import DxValidationGroup from "devextreme-vue/validation-group";
import DxValidationSummary from "devextreme-vue/validation-summary";
import DxSelectBox from "devextreme-vue/select-box";
import { useToast } from "vue-toastification";
import { DxValidator, DxRequiredRule } from "devextreme-vue/validator";
import { DxNumberBox } from "devextreme-vue/number-box";

export default {
  name: "PlayerTransferOfferAdd",
  data() {
    return {
      teams: [],
      filteredTeams: [],
      players: [],
      popupVisible: false,
      sendButtonOptionsAccept: {
        icon: "check",
        text: "Złóż ofertę",
        onClick: () => {
          this.handleSubmit();
        },
      },
      closeButtonOptions: {
        text: "Zamknij",
        onClick: () => {
          this.popupVisible = false;
          this.$emit("closed");
        },
        type: "normal",
      },
      groupRefKey: "targetGroup",
    };
  },
  computed: {
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
    }),
    ...mapFields([
      "playerTransferOffer.PlayerId",
      "playerTransferOffer.FromTeamId",
      "playerTransferOffer.ToTeamId",
      "playerTransferOffer.Content",
      "playerTransferOffer.TransferFee",
      "playerTransferOffer.PlayerEarnings",
    ]),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      addPlayerTransferOffer: "playerTransferOffers/addPlayerTransferOffer",
      setTeamsToLookup: "teams/setTeamsToLookup",
      getUserTeamId: "authentication/getUserTeamId",
      getPlayersByTeam: "matches/getPlayersByTeam",
    }),
    ...mapMutations({
      RESET_PLAYER_TRANSFER_OFFER_FORM:
        "playerTransferOffers/RESET_PLAYER_TRANSFER_OFFER_FORM",
    }),
    onToTeamChange(e) {
      this.getPlayersByTeam(e.value).then((response) => {
        this.players = response.data;
      });
    },
    async handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid) {
        await this.addPlayerTransferOffer();
        useToast().success("Oferta transferowa została złożona pomyślnie!");
        this.popupVisible = false;
        this.$emit("closed");
      }
    },
  },
  components: {
    DxPopup,
    DxToolbarItem,
    DxTextArea,
    DxValidationGroup,
    DxValidationSummary,
    DxValidator,
    DxRequiredRule,
    DxSelectBox,
    DxNumberBox,
  },
  mounted() {
    this.popupVisible = true;
    this.getUserTeamId().then((response) => {
      this.FromTeamId = response.data;
      this.setTeamsToLookup().then((response) => {
        this.teams = response.data;
        this.filteredTeams = response.data.filter(
          (x) => x.id !== this.FromTeamId
        );
      });
    });
  },
  beforeUnmount() {
    this.RESET_PLAYER_TRANSFER_OFFER_FORM();
  },
};
</script>
