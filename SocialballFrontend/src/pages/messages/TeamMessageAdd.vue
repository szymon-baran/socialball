<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="650"
      :height="650"
      container=".dx-viewport"
      title="Szczegóły meczu"
      :shading="false"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="sendButtonOptions"
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
              <label for="titleTextBox" class="form-label"
                >Tytuł wiadomości</label
              >
              <DxTextBox id="titleTextBox" v-model="Subject" />
            </div>
          </div>
          <div class="row mt-4">
            <div class="col">
              <label for="contentEditor" class="form-label"
                >Treść wiadomości</label
              >
              <DxHtmlEditor height="400" id="contentEditor" v-model="Content">
                <DxToolbar>
                  <DxItem :accepted-values="sizeValues" name="size" />
                  <DxItem :accepted-values="fontValues" name="font" />
                  <DxItem name="bold" />
                  <DxItem name="italic" />
                  <DxItem name="strike" />
                  <DxItem name="underline" />
                  <DxItem name="separator" />
                  <DxItem name="alignLeft" />
                  <DxItem name="alignCenter" />
                  <DxItem name="alignRight" />
                  <DxItem name="alignJustify" />
                  <DxItem name="separator" />
                  <DxItem name="orderedList" />
                  <DxItem name="bulletList" />
                </DxToolbar>
              </DxHtmlEditor>
            </div>
          </div>
          <div class="row">
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
import { DxHtmlEditor, DxToolbar, DxItem } from "devextreme-vue/html-editor";
import DxTextBox from "devextreme-vue/text-box";
// mapGetters, mapMutations
import { mapActions, mapGetters } from "vuex";
import { createHelpers } from "vuex-map-fields";
const { mapFields } = createHelpers({
  getterType: "messages/getField",
  mutationType: "messages/updateField",
});
import DxValidationGroup from "devextreme-vue/validation-group";
import DxValidationSummary from "devextreme-vue/validation-summary";
import { useToast } from "vue-toastification";

export default {
  name: "TeamMessageAdd",
  data() {
    return {
      popupVisible: false,
      sizeValues: ["8pt", "10pt", "12pt", "14pt", "18pt", "24pt", "36pt"],
      fontValues: [
        "Arial",
        "Courier New",
        "Georgia",
        "Impact",
        "Lucida Console",
        "Tahoma",
        "Times New Roman",
        "Verdana",
      ],
      sendButtonOptions: {
        icon: "email",
        text: "Wyślij",
        onClick: () => this.handleSubmit(),
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
      getPlayerDetails: "players/getPlayerDetails",
    }),
    ...mapFields([
      "teamMessage.FromUserId",
      "teamMessage.ToTeamId",
      "teamMessage.Subject",
      "teamMessage.Content",
      "teamMessage.SentOn",
    ]),
    validationGroup: function() {
      return this.$refs[this.groupRefKey].instance;
    },
  },
  methods: {
    ...mapActions({
      sendTeamMessage: "messages/sendTeamMessage",
      getPlayerDetailsByUserId: "players/getPlayerDetailsByUserId",
    }),
    async handleSubmit() {
      let validationResult = this.validationGroup.validate();
      if (validationResult.isValid) {
        await this.sendTeamMessage();
        useToast().success("Wiadomość drużynowa została wysłana pomyślnie!");
        this.popupVisible = false;
        this.$emit("close");
      }
    },
  },
  components: {
    DxPopup,
    DxToolbarItem,
    DxHtmlEditor,
    DxToolbar,
    DxItem,
    DxTextBox,
    DxValidationGroup,
    DxValidationSummary,
  },
  mounted() {
    this.popupVisible = true;
    this.FromUserId = this.getLoggedInUser.id;
    this.getPlayerDetailsByUserId(this.FromUserId).then(() => {
      this.ToTeamId = this.getPlayerDetails.TeamId;
    });
  },
};
</script>
