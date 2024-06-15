import Toast from "@/helpers/toast";
import router from "@/routers";
import { useAute } from "@/store/auth.module";
import axios from "axios";

const api = axios.create({
    baseURL:"https://localhost:7150/api/",
    headers:{
        "content-type": "application/json",
        "accept": "application/json",
        "Access-Control-Allow-Origin": `https://localhost:7150`,
    }
})

api.interceptors.request.use((config) => {
    config.headers.Authorization = "Bearer";
},(error) => {
    Toast.fire({
        icon: "error",
        title: error.response.data?.message,
    })
    return Promise.reject(error.response.data.message)
})

let failedQueue = [];

const processQueue = (error = null) => {
    if(error){
        failedQueue.forEach(prom => {
            prom.reject(error)
        })
    }else{
        failedQueue.forEach(prom => {
            prom.resolve(error)
        })
    }
  failedQueue = [];
}

api.interceptors.response.use((response) => {
    if (response.data.status === 201 || response.data.status === 500) {
        Toast.fire({
          icon: "error",
          title: response.data?.message,
        });
        return Promise.reject(response);
      } else if (response.data.status === 200 && response.data.message) {
        Toast.fire({
          icon: "success",
          title: response.data?.message,
        });
      } else if (response.data.status === 202 && response.data.message) {
        Toast.fire({
          icon: "error",
          title: response.data?.message,
        });
    }
    return response;
},(error) => {
    const originalRequest = error.config;
})

// // Add a response interceptor
// api.interceptors.response.use(
//     function (response) {
//       if (response.data.status === 201 || response.data.status === 500) {
//         Toast.fire({
//           icon: "error",
//           title: response.data?.message,
//         });
  
//         return Promise.reject(response);
//       } else if (response.data.status === 200 && response.data.message) {
//         Toast.fire({
//           icon: "success",
//           title: response.data?.message,
//         });
//       } else if (response.data.status === 202 && response.data.message) {
//         Toast.fire({
//           icon: "error",
//           title: response.data?.message,
//         });
//       }
  
//       return response;
//     },
  
//     function (error) {
//       const originalRequest = error.config;
//       if (error.response.status === 401 && !originalRequest?._retry) {
//         originalRequest._retry = true;
  
//         if (useAuthStore()._isTokenRefreshing) {
//           // eslint-disable-next-line promise/no-promise-in-callback
//           return new Promise(function (resolve, reject) {
//             failedQueue.push({ resolve, reject });
//           })
//             .then((res) => {
//               return api(originalRequest);
//             })
//             .catch((_error) => {
//               // eslint-disable-next-line promise/no-return-wrap
//               router.push({ name: "login" });
//               reject(_error);
//             });
//         }
  
//         return new Promise((resolve, reject) => {
//           // eslint-disable-next-line promise/no-promise-in-callback
//           useAuthStore()
//             .refreshToken()
//             .then((accessToken) => {
//               if (accessToken) {
//                 // Token refreshed successfully, update the authorization header
//                 originalRequest.headers.Authorization = `Bearer ${accessToken}`;
  
//                 // Retry the original request with the new access token
//               }
  
//               resolve(api(originalRequest));
//             })
//             .catch((error) => {
//               localStorage.setItem("redirect", router.currentRoute.value.path);
  
//               // Clear user session and redirect to login page
//               useAuthStore()._authenticated = false;
//               useAuthStore()._accessToken = null;
//               useAuthStore()._refreshToken = null;
//               useAuthStore()._user = {};
//               router.push({ name: "login" });
  
//               reject(error);
//             });
//         });
//       } else if (error.response.status === 404) {
//         router.push({ name: "not-found" });
//       } else if (error.response.status === 403) {
//         router.go(-1);
//       } else if (
//         error.response.data.message == "Unauthenticated." &&
//         error.response.status === 401
//       ) {
//         // Clear user session and redirect to login page
//         useAuthStore()._authenticated = false;
//         useAuthStore()._accessToken = null;
//         useAuthStore()._refreshToken = null;
//         useAuthStore()._user = {};
  
//         router.push({ name: "login" });
//       } else if (error.response.data.status == 401) {
//         router.push({ name: "not-authorized" });
//       }
  
//       Toast.fire({
//         icon: "error",
//         title: error.response.data?.message,
//       });
  
//       return Promise.reject(error.response.data.message);
//     }
// );


export default api