<template>
  <div>
    <course-list-item v-for="course in courses" :key="course.Id" :course="course" />
  </div>
</template>

<script>
import axios from 'axios';
import CourseListItem from './CourseListItem.vue'

export default {
  name: 'CourseList',
  components: {
    CourseListItem
  },
  data () {
    return {
      courses: null
    }
  },
  mounted () {
    axios
      .get('http://localhost:7071/api/courses')
      .then(response => (this.courses = response.data))
  }
}
</script>

