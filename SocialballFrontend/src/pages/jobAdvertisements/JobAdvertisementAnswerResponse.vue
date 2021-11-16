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
      title="Szczegóły odpowiedzi na ogłoszenie"
      :shading="false"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="sendButtonOptionsReject"
        v-if="!IsResponded"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="sendButtonOptionsAccept"
        v-if="!IsResponded"
      />
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="before"
        :options="closeButtonOptions"
      />
      <div>
        <div class="text-center" v-if="TeamId">
          <h3>Odpowiedź drużyny {{ Team.name }}</h3>
        </div>
        <div class="text-center" v-if="UserId">
          <h3>
            Odpowiedź użytkownika {{ User.userData.firstName }}
            {{ User.userData.lastName }}
          </h3>
        </div>
        <div>
          <h4>Opis:</h4>
          <p>{{ Content }}</p>
        </div>
      </div>
      <div class="mt-5">
        <h4>Potwierdzanie odpowiedzi</h4>
        <form>
          <DxValidationGroup :ref="groupRefKey">
            <div class="row mt-3">
              <div class="col">
                <label for="responseContentTextArea" class="form-label"
                  >Notatka</label
                >
                <DxTextArea
                  v-model="ResponseContent"
                  id="responseContentTextArea"
                  :auto-resize-enabled="true"
                  :max-height="90"
                  :read-only="IsResponded"
                />
              </div>
            </div>
          </DxValidationGroup>
        </form>
        <h4 class="mt-4 text-center" v-if="!IsResponded">
          Po dokonaniu wyboru, jego zmiana nie jest możliwa!
        </h4>
        <h4 class="mt-4 text-center" v-if="IsResponded">
          {{ IsResponsePositive ? "Zaakceptowano" : "Odrzucono" }} odpowiedź.
        </h4>
      </div>
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import DxTextArea from "devextreme-vue/text-area";
import { mapActions, mapMutations, mapGetters } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "jobAdvertisements/getField",
  mutationType: "jobAdvertisements/updateField",
});
import DxValidationGroup from "devextreme-vue/validation-group";
import { useToast } from "vue-toastification";
import { jobAdvertisementTypeEnum } from "../../enums/jobAdvertisementTypeEnum";

export default {
  name: "JobAdvertisementAnswerResponse",
  props: {
    jobAdvertisementAnswerId: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      jobAdvertisementTypeEnum,
      popupVisible: false,
      sendButtonOptionsReject: {
        icon: "close",
        text: "Odrzuć",
        onClick: () => {
          this.IsResponsePositive = false;
          this.handleSubmit();
        },
        type: "danger",
      },
      sendButtonOptionsAccept: {
        icon: "check",
        text: "Zaakceptuj",
        onClick: () => {
          this.IsResponsePositive = true;
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
      "answer.Id",
      "answer.JobAdvertisementId",
      "answer.JobAdvertisementAnswerType",
      "answer.Content",
      "answer.IsResponsePositive",
      "answer.TeamId",
      "answer.UserId",
      "answer.Team",
      "answer.User",
      "answer.ResponseContent",
      "answer.IsResponded",
    ]),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      updateJobAdvertisementAnswer: "jobAdvertisements/updateJobAdvertisementAnswer",
      setJobAdvertisementAnswerDetails:
        "jobAdvertisements/setJobAdvertisementAnswerDetails",
    }),
    ...mapMutations({
      RESET_JOB_ADVERTISEMENT_ANSWER_FORM:
        "jobAdvertisements/RESET_JOB_ADVERTISEMENT_ANSWER_FORM",
    }),
    async handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid) {
        await this.updateJobAdvertisementAnswer();
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
  },
  mounted() {
    this.setJobAdvertisementAnswerDetails(this.jobAdvertisementAnswerId);
    this.popupVisible = true;
  },
  beforeUnmount() {
    this.RESET_JOB_ADVERTISEMENT_ANSWER_FORM();
  },
};
</script>
