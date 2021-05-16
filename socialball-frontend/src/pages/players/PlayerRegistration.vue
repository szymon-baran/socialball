<template>
  <div class="big-data-grid">
    <h3>Rejestracja zawodnika</h3>
    <form action="/employee-page" @submit="handleSubmit">
      <DxForm :form-data="employee" :col-count="2">
        <DxGroupItem :col-count="2">
          <DxSimpleItem data-field="FirstName" :is-required="true" />
          <DxSimpleItem data-field="LastName" :is-required="true" />
          <DxSimpleItem data-field="Email">
            <DxEmailRule />
          </DxSimpleItem>
          <DxSimpleItem
            :editor-options="teamEditorOptions"
            data-field="Team"
            editor-type="dxSelectBox"
          />
        </DxGroupItem>
        <DxButtonItem :button-options="submitButtonOptions" />
      </DxForm>
    </form>
  </div>
</template>
<script>
import {
  DxForm,
  DxSimpleItem,
  DxGroupItem,
  DxButtonItem,
  DxEmailRule,
} from "devextreme-vue/form";

import { mapGetters, mapActions } from "vuex";

const employee = {
  // ...
};

const submitButtonOptions = {
  text: "Submit the Form",
  useSubmitBehavior: true,
};

export default {
  components: {
    DxForm,
    DxSimpleItem,
    DxGroupItem,
    DxButtonItem,
    DxEmailRule,
  },
  data() {
    return {
      employee,
      submitButtonOptions,
      teamEditorOptions: {
        dataSource: this.getTeams,
        value: "Id",
        displayExpr: "Name",
        valueExpr: "Id"
      },
    };
  },
  computed: {
    ...mapGetters({ getTeams: "teams/getTeams" }),
  },
  methods: {
    ...mapActions({
      setTeams: "teams/getAllTeams",
    }),

    handleSubmit(e) {
      setTimeout(() => {
        alert("Submitted");
      }, 1000);

      e.preventDefault();
    },
  },
  mounted() {
    this.setTeams();
  },
};
</script>
