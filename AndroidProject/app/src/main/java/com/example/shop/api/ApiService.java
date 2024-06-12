package com.example.shop.api;

import com.example.shop.models.Category;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;

public interface ApiService {
    @GET("api/Categories")
    Call<List<Category>> getCategories();
}
