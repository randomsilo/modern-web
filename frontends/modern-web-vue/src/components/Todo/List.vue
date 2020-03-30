<template>
  <div class="container-fluid h-100">

    <div class="row">
      <h2>Todo</h2>
    </div>

    <!-- alert box -->
    <b-row class="justify-content-md-center">
      <b-alert
        :show="alertDismissCountDown"
        dismissible
        @dismissed="alertDismissCountDown=0"
        @dismiss-count-down="alertCountDownChanged">
        <p>{{ alertMessage }}</p>
        <b-progress
          :class="{ alertCssClass }"
          :max="alertDismissSecs"
          :value="alertDismissCountDown"
          height="4px">
        </b-progress>
      </b-alert>
    </b-row>

    <!-- loading spinner -->
    <b-row class="justify-content-md-center" v-if="isLoading()">
      <b-spinner variant="primary" label="Spinning"></b-spinner>
    </b-row>

    <!-- title bar -->
    <div class="row">
      <div class="card col-12">
        <div class="card-header">
          <ul class="nav nav-tabs card-header-tabs">
            <li class="nav-item">
              <a class="nav-link" href="#" :class="isTableVisible() ? 'active' : ''" @click="showTable()">Listing</a>
            </li>
            <li class="nav-item" >
              <a class="nav-link" href="#" :class="isFormVisible() ? 'active' : ''" @click="showForm()">Form</a>
            </li>
          </ul>
        </div>
        
        <div class="card-body">
            
          <!-- table listing -->
          <div class="row" v-if="isTableVisible()">
            <table class="table">
              <thead class="thead-light">
                <tr>
                  <th scope="col" colspan="4">
                    <button class="btn btn-success btn-sm float-right" title="add new" @click="showForm()"><b-icon icon="plus"></b-icon></button>
                    <button class="btn btn-primary btn-sm float-right" title="refresh listing" @click="onGetListing(true)"><b-icon icon="arrow-repeat"></b-icon></button>
                  </th>
                </tr>
              </thead>
              <thead class="thead-dark">
                <tr>
                  <th scope="col">Action</th>
                  <th scope="col">ID</th>
                  <th scope="col">Summary</th>
                  <th scope="col">Details</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for='(item, index) in this.items' :key='item.todoEntryId'>
                  <th scope="row">
                    <button class="btn btn-danger btn-sm float-right" title="remove" @click="onRemove(index)"><b-icon icon="trash"></b-icon></button>
                    <button class="btn btn-secondary btn-sm float-right" title="edit" @click="onEdit(index)"><b-icon icon="pencil"></b-icon></button>
                  </th>
                  <td>{{ item.todoEntryId }}</td>
                  <td>{{ item.summary }}</td>
                  <td>{{ item.details }}</td>
                </tr>
              </tbody>
            </table>
          </div>

          <!-- form -->
          <div class="row" v-if="isFormVisible()">
            <form class="col-12" @submit.prevent="onSave">

              <div class="form-group row">
                <label for="todoEntryId" class="col-3">ID</label>
                <input v-model="form.todoEntryId" type="text" id="todoEntryId" class="form-control form-control-sm col" autocomplete="off" readonly>
              </div>

              <div class="form-group row">
                <label for="summary" class="col-3">Summary</label>
                <input v-model="form.summary" type="text" id="summary" class="form-control form-control-sm col" autocomplete="off" required autofocus>
              </div>

              <div class="form-group row">
                <label for="details" class="col-3">Details</label>
                <input v-model="form.details" type="text" id="details" class="form-control form-control-sm col" autocomplete="off">
              </div>

              <div class="form-group row">
                <div class="col-12">
                  <button type="reset" class="btn btn-warning btn-sm float-right" title="clear" @click="onReset"><b-icon icon="x-square"></b-icon></button>
                  <button type="submit" class="btn btn-primary btn-sm float-right">Save</button>
                </div>
              </div>

            </form>
          </div>


        </div>
      </div>
    </div>



    <div class="row">
      <br />
      <br />
    </div>

    

    

    <!-- form debug -->
    <b-row v-if="isFormVisible()">
      <b-card class="mt-3" header="Form Data Result">
        <pre class="m-0">{{ form }}</pre>
      </b-card>
    </b-row>
  </div>
</template>

<script>
/* eslint-disable */
export default {
  name: 'TodoList'
  , mounted() {
    this.onGetListing(true);
  }

  , data() {

    return {
      tableRefreshNeeded: false,
      tableVisible: true,
      tableLoading: false,
      alert: false,
      alertMessage: "",
      alertCssClass: "secondary",
      alertDismissSecs: 5,
      alertDismissCountDown: 0,
      perPage: 20,
      currentPage: 1,
      fields: [
        { key: 'todoEntryId', label: 'ID' },
        { key: 'summary', label: 'Summary' },
        { key: 'details', label: 'Details' }
      ],
      items: [
      ],
      form: {
        todoEntryId: '',
        summary: '',
        details: null
      },
      foods: [{ text: 'Select One', value: null }, 'Carrots', 'Beans', 'Tomatoes', 'Corn'],
      formVisible: true
    }
  }

  , computed: {
    rows() {
      return this.items.length
    }
  }

  , methods: {
    toggleTableVisible() {
      this.tableVisible = !this.tableVisible;
    }

    , showTable() {
      this.tableVisible = true;

      if (this.tableRefreshNeeded) {
        this.onGetListing(false);
      }
    }

    , showForm() {
      this.tableVisible = false;
    }

    , isTableVisible() {
      return this.tableVisible;
    }

    , isFormVisible() {
      return !this.tableVisible;
    }

    , isLoading() {
      return this.tableLoading;
    }

    , setAlert(message, cssClass) {
      if (message == false) {
        this.alertDismissCountDown = 0;
        this.alertMessage = "";
      } else {
        this.alertDismissCountDown = this.alertDismissSecs;
        this.alertMessage = message;
      }

      this.alertCssClass = "secondary";
      if (cssClass != null && cssClass.length > 0) {
        this.alertCssClass = cssClass;
      }
    }

    , alertCountDownChanged(dismissCountDown) {
      this.alertDismissCountDown = dismissCountDown;
    }

    , onGetListing(showStatusAlert) {
      this.tableLoading = true;

      this.TodoListApi.get('/TodoEntry')
        .then(request => this.onGetListingSuccess(request, showStatusAlert))
        .catch(() => this.onGetListingFail())
    }

    , onGetListingSuccess (request, showStatusAlert) {
      this.items = request.data;

      if (this.items.length == 0) {
        if (showStatusAlert) {
          this.setAlert("No data found.", "warning");
        }
        this.tableVisible = false;
      }
      else {
        if (showStatusAlert) {
          this.setAlert("Loaded " + this.rows + " records", "secondary");
        }
        this.tableVisible = true;
      }

      this.tableLoading = false;
      this.tableRefreshNeeded = false;
    }

    , onGetListingFail() {
      this.tableLoading = false;
      this.setAlert("An error has occurred.", "danger");
    }

    , onSave(evt) {
      evt.preventDefault();

      if (this.form.todoEntryId == '') {
        // add
        this.TodoListApi.post('/TodoEntry', 
        {
          "todoEntryId": null,
          "summary": this.form.summary,
          "details": this.form.details,
          "dueDate": null,
          "entryStatusIdRef": null
        })
          .then(request => this.onSaveSuccess(request))
          .catch(() => this.onSaveFail());
      }
      else {
        // edit
        this.TodoListApi.put('/TodoEntry/'+this.form.todoEntryId, 
        {
          "todoEntryId": this.form.todoEntryId,
          "summary": this.form.summary,
          "details": this.form.details,
          "dueDate": null,
          "entryStatusIdRef": null
        })
          .then(request => this.onSaveSuccess(request))
          .catch(() => this.onSaveFail());
      }
    }

    , onSaveSuccess(req) {
      this.setAlert("Saved!", "success");
      this.tableRefreshNeeded = true;
    }

    , onSaveFail() {
      this.setAlert("An error has occurred.", "danger");
    }
    
    , onReset(evt) {
      evt.preventDefault();

      // Reset our form values
      this.form.todoEntryId = '';
      this.form.summary = '';
      this.form.details = '';

      // Trick to reset/clear native browser form validation state
      this.formVisible = false;
      this.$nextTick(() => {
        this.formVisible = true;
      });

    }

    , onEdit(idx) {
      var item = this.items[idx];
      if (item) {
        // Set form values
        this.form.todoEntryId = item.todoEntryId;
        this.form.summary = item.summary;
        this.form.details = item.details;

        this.showForm();
      }
      else {
        this.setAlert("An error has occurred.", "danger");
      }
    }

    , onRemove(idx) {
      var item = this.items[idx];
      if (item) {
        this.TodoListApi.delete('/TodoEntry/'+item.todoEntryId)
          .then(request => this.onRemoveSuccess(request))
          .catch(() => this.onRemoveFail());
      }
      else {
        this.setAlert("An error has occurred.", "danger");
      }
    }

    , onRemoveSuccess(req) {
      this.onGetListing(false);
      this.setAlert("Removed!", "success");
    }

    , onRemoveFail() {
      this.setAlert("An error has occurred.", "danger");
    }
    
  }
}
</script>

<style lang="css">

</style>