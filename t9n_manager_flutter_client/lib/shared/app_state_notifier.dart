import 'package:flutter/material.dart';

class AppState with ChangeNotifier {
//Constant
  AppState() {}
  double screenMaxSize = 600;
  double cardSize = 100;
  bool isLoggedIn = false;
  String jwt = '';

  void toggleLogedIn() {
    isLoggedIn = !isLoggedIn;
    notifyListeners();
  }

  void setToken(String token) {
    jwt = token;
    notifyListeners();
  }
}
