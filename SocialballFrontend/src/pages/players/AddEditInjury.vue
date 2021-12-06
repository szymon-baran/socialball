<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="400"
      :height="300"
      container=".dx-viewport"
      :title="getTitle()"
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
              <label for="playersSelectBox" class="form-label">Pozycja</label>
              <DxSelectBox
                :dataSource="players"
                value-expr="id"
                display-expr="name"
                v-model="Id"
                id="playersSelectBox"
                :read-only="isPlayerReadOnly"
              >
                <DxValidator>
                  <DxRequiredRule message="Wybierz zawodnika" />
                </DxValidator>
              </DxSelectBox>
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <label for="dateTimeTextBox" class="form-label"
                >Przewidywana data zakończenia kontuzji</label
              >
              <DxDateBox
                id="dateTimeTextBox"
                v-model="IsInjuredUntil"
                type="date"
                display-format="dd/MM/yyyy"
                cancel-button-text="Anuluj"
                invalid-date-message="Wartość musi być datą lub czasem"
              >
                <DxValidator>
                  <DxRequiredRule message="Podaj datę kontuzji" />
                </DxValidator>
              </DxDateBox>
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
import { DxValidator, DxRequiredRule } from "devextreme-vue/validator";
import DxValidationGroup from "devextreme-vue/validation-group";
import DxValidationSummary from "devextreme-vue/validation-summary";
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import DxSelectBox from "devextreme-vue/select-box";
import DxDateBox from "devextreme-vue/date-box";

import { mapActions, mapMutations, mapGetters } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "players/getField",
  mutationType: "players/updateField",
});

import { useToast } from "vue-toastification";

export default {
  name: "AddEditInjury",
  props: {
    showToEdit: {
      type: Boolean,
      default: false,
    },
    userId: {
      type: String,
      required: false,
    },
  },
  data() {
    return {
      popupVisible: false,
      positions: [],
      players: [],
      userTeamId: "",
      isPlayerReadOnly: false,
      sendButtonOptionsAccept: {
        icon: "check",
        text: "Zapisz",
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
    ...mapFields(["player.Id", "player.IsInjuredUntil"]),
    ...mapGetters({
      isLoggedIn: "authentication/isLoggedIn",
    }),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      setPlayerDetails: "players/setPlayerDetails",
      getPlayersByTeam: "matches/getPlayersByTeam",
      addEditPlayerInjury: "players/addEditPlayerInjury",
      getUserTeamId: "authentication/getUserTeamId",
    }),
    ...mapMutations({
      RESET_PLAYER_FORM: "players/RESET_PLAYER_FORM",
    }),
    getTitle() {
      return (this.showToEdit ? "Edytuj" : "Dodaj") + " kontuzję zawodnika";
    },
    handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid && this.showToEdit) {
        this.addEditPlayerInjury(this.userTeamId).then(() => {
          this.popupVisible = false;
          this.$emit("closed");
          useToast().success("Edycja kontuzji zakończona pomyślnie!");
        });
      } else if (validationResult.isValid && !this.showToEdit) {
        this.addEditPlayerInjury(this.userTeamId).then(() => {
          this.popupVisible = false;
          this.$emit("closed");
          useToast().success("Dodawanie kontuzji zakończone pomyślnie!");
        });
      }
    },
  },
  mounted() {
    if (this.isLoggedIn) {
      this.getUserTeamId().then((response) => {
        this.userTeamId = response.data;
        this.getPlayersByTeam(response.data).then((resp) => {
          this.players = resp.data;
        });
      });
    }

    this.popupVisible = true;
    if (this.showToEdit) {
      this.isPlayerReadOnly = true;
      this.setPlayerDetails(this.userId);
    }
  },
  components: {
    DxValidator,
    DxRequiredRule,
    DxValidationGroup,
    DxValidationSummary,
    DxPopup,
    DxToolbarItem,
    DxSelectBox,
    DxDateBox,
  },
  beforeUnmount() {
    this.RESET_PLAYER_FORM();
  },
};
</script>
