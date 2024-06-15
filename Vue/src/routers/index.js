import { useAute } from '@/store/auth.module';
import {createRouter,createWebHistory} from 'vue-router'

const router = createRouter({
    history:createWebHistory(),
    routes:[
        {
            path:'/',
            component:() => import("@/layouts/defualt.vue"),
            children:[
                {
                    path:'/',
                    redirect:{
                        name:'home'
                    }
                },
                {
                    path:'/dashboard',
                    name:'home',
                    component:() => import("@/pages/dashboard.vue")
                }
            ]
        },
        {
            path:'/login',
            name:'login',
            component:() => import("@/pages/login.vue")
        }
    ]
})

// router.beforeEach((to,from,next) => {

//     const authStore = useAute()
//     if (authStore._authenticated) {
//         authStore.fetchNotification();
//       }
    
//       if (isLoggedIn) {
//         if (to.meta.development) {
//           return { name: "under-development" };
//         }
    
//         if (to.name == "login" || to.path == "/") {
//           return { name: "dashboards" };
//         }
//       } else {
//         if (to.name != "login") {
//           return { name: "login" };
//         }
//       }  
// })

export default router;