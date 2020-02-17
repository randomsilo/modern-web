<template>
  <b-container>
    <b-row class="justify-content-md-center">
      <b-col cols="10">
        <h2>{{ tableName }} </h2>
      </b-col>
      <b-col cols="2">
        <b-button-group>
          <b-button
            class="btn btn-secondary"
            size="sm">
            Rows {{ rows }}
          </b-button>
          <b-button 
            class="btn btn-success"
            size="sm"
            @click="getListing()">
            <b-icon-arrow-repeat></b-icon-arrow-repeat> 
          </b-button>
        </b-button-group>
      </b-col>
    </b-row>
    <b-row class="justify-content-md-center">
    
      <b-table
        id="listing"
        :items="items"
        :fields="fields"
        :per-page="perPage"
        :current-page="currentPage"
        small
      ></b-table>

      <b-pagination
        v-model="currentPage"
        :total-rows="rows"
        :per-page="perPage"
        aria-controls="listing"
      ></b-pagination>
    </b-row>
  </b-container>
</template>

<script>
/* eslint-disable */
export default {
  name: 'TodoList'
  , mounted() {
    this.getListing();
  }

  , data() {

    return {
      tableName: 'Todo List',
      perPage: 20,
      currentPage: 1,
      fields: [
        { key: 'todoEntryId', label: 'ID' },
        { key: 'summary', label: 'Summary' },
        { key: 'details', label: 'Details' }
      ],
      items: [
      ]
    }
  }

  , computed: {
    rows() {
      return this.items.length
    }
  }

  , methods: {
    getListing() {
      this.TodoListApi.get('/TodoEntry')
        .then(request => this.handleListingResponse(request))
        .catch(() => this.handleListingError())
    }

    , handleListingResponse (request) {
      this.items = request.data;
    }

    , handleListingError() {
    }

  }
}
</script>

<style lang="css">

</style>