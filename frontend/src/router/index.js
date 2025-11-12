import { createRouter, createWebHistory } from "vue-router"
import LoginView from "../views/LoginView.vue"
import GeneralDashboardView from "../views/GeneralDashboardView.vue"
import AdminDashboardView from "../views/AdminDashboardView.vue"
import HomeView from "../views/HomeView.vue"
import RegisterView from "../views/RegisterView.vue"


const routes = [
  {
    path: "/",
    component: HomeView
  },
  {
    path: "/login",
    component: LoginView
  },
  {
    path: "/register",
    component: RegisterView
  },
  {
    path: "/dashboard/general",
    component: GeneralDashboardView,
    meta: { requiresAuth: true },
  },
  {
    path: "/dashboard/admin",
    component: AdminDashboardView,
    meta: { requiresAuth: true },
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) =>{
  const token = localStorage.getItem("token")

  if (to.meta.requiresAuth && !token) {
    next("/login");
  } else {
    next()
  }
})

export default createRouter({
  history: createWebHistory(),
  routes
})