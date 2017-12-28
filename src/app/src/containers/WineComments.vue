<template>
  <div class="wine-comments">
    <div v-for="comment in comments" :key="comment.WineId">
      {{comment.Note}}
    </div>
  </div>
</template>

<script>
import commentsService from "../services/comments";

export default {
  name: "WineComments",
  props: {
    vinmonopoletId: String
  },
  data() {
    return {
      comments: [],
      error: ""
    };
  },
  mounted() {
    commentsService
      .get(this.vinmonopoletId)
      .then(response => {
        this.comments = response.data;
      })
      .catch(e => {
        this.error = e;
      });
  }
};
</script>

<style scoped>
.wine-comments {
  border: solid;
  padding-left: 10px;
  padding-bottom: 10px;
}
</style>
