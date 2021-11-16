<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="500"
      :height="550"
      container=".dx-viewport"
      title="Odpowiedz na ogłoszenie"
      :shading="false"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="sendButtonOptionsAccept"
        v-if="!isAlreadyAnswered"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="closeButtonOptions"
      />
      <div
        v-if="jobAdvertisementAnswerType == jobAdvertisementTypeEnum.FROM_USER"
      >
        <div class="text-center">
          <h3>Oferta drużyny {{ jobAdvertisement.TeamName }}</h3>
          <h4>
            Szukają zawodnika na pozycję, którą ustawiłeś jako preferowaną!
          </h4>
        </div>
        <ul class="mt-4">
          <li>Lokalizacja: {{ jobAdvertisement.Location }}</li>
          <li>Zarobki pomiędzy: {{ jobAdvertisement.Earnings }}</li>
          <li>
            Liczba sesji treningowych w tygodniu:
            {{ jobAdvertisement.TrainingSessionsPerWeek }}
          </li>
          <li>
            Opis:
            <p>{{ jobAdvertisement.Content }}</p>
          </li>
        </ul>
      </div>
      <div
        v-if="jobAdvertisementAnswerType == jobAdvertisementTypeEnum.FROM_TEAM"
      >
        <div class="text-center">
          <h3>
            Zawodnik {{ jobAdvertisement.UserFullName }} poszukuje drużyny!
          </h3>
        </div>
        <ul class="mt-4">
          <li>Lokalizacja: {{ jobAdvertisement.Location }}</li>
          <li>
            Opis:
            <p>{{ jobAdvertisement.Content }}</p>
          </li>
        </ul>
      </div>
      <div class="mt-5" v-if="!isAlreadyAnswered">
        <h4>Składanie podania</h4>
        <form>
          <DxValidationGroup :ref="groupRefKey">
            <div class="row">
              <div class="col">
                <label for="contentTextArea" class="form-label">Treść</label>
                <DxTextArea
                  v-model="Content"
                  id="contentTextArea"
                  :auto-resize-enabled="true"
                  :max-height="90"
                >
                  <DxValidator>
                    <DxRequiredRule message="Wprowadź treść podania!" />
                  </DxValidator>
                </DxTextArea>
              </div>
            </div>
            <div class="row">
              <div class="col">
                <DxValidationSummary />
              </div>
            </div>
          </DxValidationGroup>
        </form>
        <h4 class="mt-4 text-center" v-if="!isAlreadyAnswered">
          Możesz złożyć odpowiedź na to ogłoszenie. Jeżeli zostanie rozpatrzone
          pozytywnie, system
          <span v-if="jobAdvertisementAnswerType == jobAdvertisementTypeEnum.FROM_USER">
            doda Cię do drużyny.
          </span>
          <span v-if="jobAdvertisementAnswerType == jobAdvertisementTypeEnum.FROM_TEAM">
            doda zawodnika do Twojej drużyny.
          </span>
        </h4>
      </div>
      <h4 class="mt-5 text-center" v-if="isAlreadyAnswered">
        Już złożyłeś odpowiedź na to ogłoszenie. Teraz wystarczy czekać na
        kontakt od drugiej strony. Powodzenia!
      </h4>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import DxTextArea from "devextreme-vue/text-area";
import { mapActions, mapMutations, mapState, mapGetters } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "jobAdvertisements/getField",
  mutationType: "jobAdvertisements/updateField",
});
import DxValidationGroup from "devextreme-vue/validation-group";
import DxValidationSummary from "devextreme-vue/validation-summary";
import { useToast } from "vue-toastification";
import { jobAdvertisementTypeEnum } from "../../enums/jobAdvertisementTypeEnum";
import { DxValidator, DxRequiredRule } from "devextreme-vue/validator";

export default {
  name: "JobAdvertisementAnswerAdd",
  props: {
    jobAdvertisementId: {
      type: String,
      required: true,
    },
    jobAdvertisementAnswerType: {
      type: Number,
      required: true,
    },
    isAlreadyAnswered: {
      type: Boolean,
      default: false,
    },
  },
  data() {
    return {
      jobAdvertisementTypeEnum,
      teams: [],
      popupVisible: false,
      sendButtonOptionsAccept: {
        icon: "check",
        text: "Wyślij odpowiedź",
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
    ...mapState("jobAdvertisements", ["jobAdvertisement"]),
    ...mapGetters({
      getLoggedInUser: "authentication/getLoggedInUser",
    }),
    ...mapFields([
      "answer.JobAdvertisementId",
      "answer.JobAdvertisementAnswerType",
      "answer.Content",
      "answer.TeamId",
      "answer.UserId",
    ]),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      addJobAdvertisementAnswer: "jobAdvertisements/addJobAdvertisementAnswer",
      setJobAdvertisementDetails:
        "jobAdvertisements/setJobAdvertisementDetails",
      getTeamsToLookup: "jobAdvertisements/getTeamsToLookup",
    }),
    ...mapMutations({
      RESET_JOB_ADVERTISEMENT_ANSWER_FORM:
        "jobAdvertisements/RESET_JOB_ADVERTISEMENT_ANSWER_FORM",
    }),
    async handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid) {
        await this.addJobAdvertisementAnswer();
        useToast().success(
          "Odpowiedź na zgłoszenie została wysłana pomyślnie!"
        );
        this.popupVisible = false;
        this.$emit("close");
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
  },
  mounted() {
    this.setJobAdvertisementDetails(this.jobAdvertisementId);
    this.getTeamsToLookup().then((response) => {
      this.teams = response.data;
    });

    this.popupVisible = true;
    this.JobAdvertisementId = this.jobAdvertisementId;
    this.JobAdvertisementAnswerType = this.jobAdvertisementAnswerType;
    this.UserId = this.getLoggedInUser.id;
  },
  beforeUnmount() {
    this.RESET_JOB_ADVERTISEMENT_ANSWER_FORM();
  },
};
</script>
<style scoped>
ul {
  font-size: 105%;
}
</style>
