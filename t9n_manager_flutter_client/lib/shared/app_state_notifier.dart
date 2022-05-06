import 'package:flutter/material.dart';

class AppState with ChangeNotifier {
//Constant
  AppState() {}
  double screenMaxSize = 600;
  double cardSize = 100;
  bool isLoggedIn = false;

  void toggleLogedIn() {
    isLoggedIn = !isLoggedIn;
    notifyListeners();
  }
}
