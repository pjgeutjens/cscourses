<template>
  <b-card
    :title=course.title
    tag="article"
    style="max-width: 20rem;"
    class="mb-2"
  >
    <b-card-text>
      {{course.description}}
    </b-card-text>

    <b-button href="#" variant="primary">Details</b-button>
  </b-card>
</template>

<script>
export default {
  name: 'CourseListItem',
  props: {
    course: Object
  }
}
</script>

