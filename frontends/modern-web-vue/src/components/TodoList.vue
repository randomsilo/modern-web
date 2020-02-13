<template>
  <div class="overflow-auto">
    <h2>{{ tableName }} <small>({{ rows }}) rows</small></h2>
    <button class="btn btn-lg btn-primary btn-block" @click="getListing()">Refresh</button>
    
    <b-pagination
      v-model="currentPage"
      :total-rows="rows"
      :per-page="perPage"
      aria-controls="listing"
    ></b-pagination>

    <b-table
      id="listing"
      :items="items"
      :per-page="perPage"
      :current-page="currentPage"
      small
    ></b-table>
  </div>
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
      perPage: 5,
      currentPage: 1,
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