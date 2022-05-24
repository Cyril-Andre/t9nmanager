import 'package:flutter/material.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/models/tenant.dart';

class AppState with ChangeNotifier {
//Constant
  AppState();
  double screenMaxSize = 600;
  double cardSize = 150;
  bool isLoggedIn = false;
  String jwt = '';
  Tenant selectedTenant = Tenant("_", "Public","PublicAdmin");
  String selectedProject = "(none)";

  void logIn() {
    isLoggedIn = true;
    notifyListeners();
  }

  void logOut() {
    isLoggedIn = false;
    notifyListeners();
  }
/*
  void toggleLogedIn() {
    isLoggedIn = !isLoggedIn;
    notifyListeners();
  }
*/
  void setToken(String token) {
    jwt = token;
    notifyListeners();
  }

  void setTenant(Tenant tenant) {
    selectedTenant = tenant;
    notifyListeners();
  }
}
