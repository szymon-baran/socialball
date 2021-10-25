<template>
  <div>
    <DxPopup
      v-model:visible="popupVisible"
      :drag-enabled="false"
      :close-on-outside-click="false"
      :show-close-button="false"
      :show-title="true"
      :width="650"
      :height="800"
      container=".dx-viewport"
      title="Szczegóły meczu"
      :shading="false"
    >
      <DxToolbarItem
        widget="dxButton"
        toolbar="bottom"
        location="after"
        :options="closeButtonOptions"
      />
      <DxHtmlEditor height="660">
        <DxToolbar>
          <DxItem :accepted-values="sizeValues" name="size" />
          <DxItem :accepted-values="fontValues" name="font" />
          <DxItem name="separator" />
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
    </DxPopup>
  </div>
</template>
<script>
import { DxPopup, DxToolbarItem } from "devextreme-vue/popup";
import { DxHtmlEditor, DxToolbar, DxItem } from "devextreme-vue/html-editor";

// import { mapActions, mapGetters, mapMutations } from "vuex";
// import { createHelpers } from "vuex-map-fields";
// const { mapFields } = createHelpers({
//   getterType: "players/getField",
//   mutationType: "players/updateField",
// });

export default {
  name: "TeamMessageAdd",
  props: {
    isAddTeamMessagePopupVisible: {
      type: Boolean,
      default: false,
    },
  },
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
      closeButtonOptions: {
        text: "Zamknij",
        onClick: () => {
          this.popupVisible = false;
          this.$emit("closed");
        },
      },
    };
  },
  components: {
    DxPopup,
    DxToolbarItem,
    DxHtmlEditor,
    DxToolbar,
    DxItem,
  },
  mounted() {
    this.popupVisible = this.isAddTeamMessagePopupVisible;
  },
};
</script>
