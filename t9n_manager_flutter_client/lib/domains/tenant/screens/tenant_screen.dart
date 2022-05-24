import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/apis/api_tenant.dart';
import 'package:t9n_manager_flutter_client/shared/alert.dart';
import 'package:t9n_manager_flutter_client/shared/app_state_notifier.dart';
import 'package:t9n_manager_flutter_client/shared/token_helper.dart';
import '../../../generated/l10n.dart';
import '../../../shared/app_settings.dart';
import '../../../shared/models/api_exception.dart';
import '../models/tenant.dart';
import '../widgets/tenant_card.dart';

class TenantScreen extends StatefulWidget {
  const TenantScreen({Key? key}) : super(key: key);

  @override
  State<TenantScreen> createState() => _TenantScreenState();
}

class _TenantScreenState extends State<TenantScreen> {
  bool toBeRefreshed = false;
  @override
  void initState() {
    toBeRefreshed = false;
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    AppSettings appSettings = context.watch<AppSettings>();
    AppState appState = context.watch<AppState>();
    var jwt = appState.jwt;
    double width = MediaQuery.of(context).size.width > context.watch<AppState>().screenMaxSize ? context.watch<AppState>().screenMaxSize : MediaQuery.of(context).size.width;
    double cardSize = context.watch<AppState>().cardSize;
    int crossAxisCount = (width / cardSize).floor();

    return FutureBuilder<List<Tenant>>(
        future: getAllTenants(appSettings, jwt, context),
        builder: (context, AsyncSnapshot<List<Tenant>> snapshot) {
          if (snapshot.connectionState != ConnectionState.done) {
            return const CircularProgressIndicator();
          } else if (snapshot.hasData) {
            return Scaffold(
              appBar: AppBar(
                title: Text(S.of(context).title),
              ),
              body: getTenantCards(snapshot.requireData, crossAxisCount),
            );
          } else if (snapshot.hasError) {
            ApiException e = snapshot.error as ApiException;
            if (e.code == 401) {
              return alert(S.of(context).api_error_title, e.t9nMessage, S.of(context).common_button_ok, "/relog", context);
            } else {
              return alert(S.of(context).api_error_title, e.t9nMessage, S.of(context).common_button_ok, "", context);
            }
          } else {
            return const Text("");
          }
        });
  }

  Widget getTenantCards(List<Tenant> tenants, int crossAxisCount) {
    try {
      List<Widget> listTenantCards = <Widget>[];
      for (var counter = 0; counter < tenants.length; counter++) {
        listTenantCards.add(TenantCard(tenant: tenants[counter], addNew: false, refresh: refresh));
      }
      listTenantCards.add(TenantCard(tenant: Tenant("_", S.of(context).tenant_add_button,'PublicAdmin'), addNew: true, refresh: refresh));
      return GridView.count(
        crossAxisCount: crossAxisCount,
        padding: const EdgeInsets.all(5.0),
        children: listTenantCards,
      );
    } catch (e) {
      rethrow;
    }
  }

  refresh() {
    setState(() {
      toBeRefreshed = !toBeRefreshed;
    });
  }
}
